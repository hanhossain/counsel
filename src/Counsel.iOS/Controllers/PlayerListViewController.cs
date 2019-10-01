using System;
using System.Collections.Generic;
using System.Linq;
using Counsel.Core;
using Counsel.Core.Models;
using Counsel.iOS.Views;
using Foundation;
using UIKit;

namespace Counsel.iOS.Controllers
{
	public sealed class PlayerListViewController : UITableViewController
	{
		private const string _cellId = "playerListCell";

		private readonly IFantasyService _fantasyService;
		private readonly List<(char Initial, int Offset)> _sections = new List<(char, int)>();
		
		private List<Player> _players = new List<Player>();

		public PlayerListViewController(IFantasyService fantasyService)
		{
			_fantasyService = fantasyService;
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.RegisterClassForCellReuse<SubtitleTableViewCell>(_cellId);

			var loadingAlert = UIAlertController.Create("Loading...", null, UIAlertControllerStyle.Alert);
			BeginInvokeOnMainThread(() => PresentViewController(loadingAlert, true, null));

			(int season, int week) = await _fantasyService.GetCurrentWeekAsync();
			BeginInvokeOnMainThread(() => Title = $"Season: {season} - Week: {week}");

			var players = await _fantasyService.GetPlayersAsync();
			_players = players.Values
				.OrderBy(x => x.LastName)
				.ThenBy(x => x.FirstName)
				.ToList();

			for (int i = 0; i < _players.Count; i++)
			{
				var player = _players[i];
				char playerInitial = player.LastName.First();

				if (char.IsNumber(playerInitial))
				{
					playerInitial = '#';
				}

				if (_sections.Count == 0 || playerInitial != _sections[_sections.Count - 1].Initial)
				{
					_sections.Add((playerInitial, i));
				}
			}

			BeginInvokeOnMainThread(() =>
			{
				DismissViewController(true, null);
				TableView.ReloadData();
			});
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return _sections.Count;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (!_sections.Any())
			{
				return 0;
			}

			var sectionOffset = _sections[(int)section].Offset;
			var exclusiveUpperBound = section < _sections.Count - 1
				? _sections[(int)section + 1].Offset
				: _players.Count;

			return exclusiveUpperBound - sectionOffset;
		}

		public override string[] SectionIndexTitles(UITableView tableView)
		{
			return _sections
				.Select(x => x.Initial.ToString())
				.ToArray();
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell<SubtitleTableViewCell>(_cellId, indexPath);
			var player = GetPlayer(indexPath);

			cell.TextLabel.Text = player.FullName;
			cell.DetailTextLabel.Text = $"{player.Position} - {player.Team}";
			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var player = GetPlayer(indexPath);
			var playerDetailViewController = new PlayerDetailViewController(player, _fantasyService);
			NavigationController.PushViewController(playerDetailViewController, true);
		}

		private Player GetPlayer(NSIndexPath indexPath)
		{
			var playerIndex = _sections[indexPath.Section].Offset + indexPath.Row;
			var player = _players[playerIndex];
			return player;
		}
	}
}
