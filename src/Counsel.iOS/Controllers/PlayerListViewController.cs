using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Counsel.Core;
using Counsel.Core.Models;
using Counsel.iOS.Views;
using Foundation;
using UIKit;

namespace Counsel.iOS.Controllers
{
	public sealed class PlayerListViewController : UITableViewController, ISearchDelegate
	{
		private const string _cellId = "playerListCell";

		private readonly IFantasyService _fantasyService;
		private readonly List<(char Initial, int Offset)> _sections = new List<(char, int)>();

		private List<Player> _players = new List<Player>();
		private UIBarButtonItem _resetButton;

		public PlayerListViewController(IFantasyService fantasyService)
		{
			_fantasyService = fantasyService;
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.RegisterClassForCellReuse<SubtitleTableViewCell>(_cellId);

			var searchButton = new UIBarButtonItem(UIBarButtonSystemItem.Search);
			searchButton.Clicked += SearchButton_Clicked;

			NavigationItem.RightBarButtonItem = searchButton;

			_resetButton = new UIBarButtonItem()
			{
				Title = "Reset"
			};
			_resetButton.Clicked += async (s, e) =>
			{
				await LoadAllPlayersAsync();
				InvokeOnMainThread(() => TableView.ReloadData());
			};

			var loadingAlert = UIAlertController.Create("Loading...", null, UIAlertControllerStyle.Alert);
			InvokeOnMainThread(() => PresentViewController(loadingAlert, true, null));

			await _fantasyService.UpdateAsync();
			await LoadCurrentWeekAsync();
			await LoadAllPlayersAsync();

			InvokeOnMainThread(() =>
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

			int sectionOffset = _sections[(int)section].Offset;
			int exclusiveUpperBound = section < _sections.Count - 1
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
			int playerIndex = _sections[indexPath.Section].Offset + indexPath.Row;
			var player = _players[playerIndex];
			return player;
		}

		private async Task LoadCurrentWeekAsync()
		{
			(int season, int week) = await _fantasyService.GetCurrentWeekAsync();
			BeginInvokeOnMainThread(() => Title = $"Season: {season} - Week: {week}");
		}

		private async Task LoadAllPlayersAsync()
		{
			var players = await _fantasyService.GetPlayersAsync();
			LoadPlayers(players);

			InvokeOnMainThread(() => NavigationItem.SetLeftBarButtonItem(null, true));
		}

		private void LoadPlayers(Dictionary<string, Player> players)
		{
			_players = players.Values
				.OrderBy(x => x.LastName)
				.ThenBy(x => x.FirstName)
				.ToList();

			LoadSections();
		}

		private void LoadSections()
		{
			_sections.Clear();

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
		}

		private void SearchButton_Clicked(object sender, EventArgs e)
		{
			var navController = new UINavigationController(new SearchViewController(this));
			PresentViewController(navController, true, null);
		}

		public async Task OnSearchAsync(string playerName)
		{
			var players = await _fantasyService.SearchPlayersAsync(playerName);
			LoadPlayers(players);

			InvokeOnMainThread(() =>
			{
				NavigationItem.SetLeftBarButtonItem(_resetButton, true);
				TableView.ReloadData();
			});
		}
	}
}
