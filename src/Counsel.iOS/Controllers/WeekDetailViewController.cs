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
	public class WeekDetailViewController : UITableViewController
	{
		private const string _cellId = "weekDetailCell";

		private readonly IFantasyService _fantasyService;
		private readonly Player _player;
		private readonly PlayerStats _playerStats;
		private readonly int _week;

		private Dictionary<string, List<(Player Player, double Points)>> _opponents = new Dictionary<string, List<(Player Player, double Points)>>();
		private List<string> _positions = new List<string>();

		public WeekDetailViewController(IFantasyService fantasyService, Player player, PlayerStats playerStats, int week)
		{
			_fantasyService = fantasyService;
			_player = player;
			_playerStats = playerStats;
			_week = week;
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.RegisterClassForCellReuse<RightDetailTableViewCell>(_cellId);
			Title = $"Week: {_week} - {_player.Team}";

			_opponents = await _fantasyService.GetOpponentsAsync(_player.Id, _playerStats.Season, _week);
			_positions = _opponents.Keys.OrderBy(x => x).ToList();

			string opponentTeam = _opponents.Values.FirstOrDefault()?.FirstOrDefault().Player?.Team;

			InvokeOnMainThread(() =>
			{
				Title = $"Week: {_week} - {_player.Team} vs {opponentTeam}";
				TableView.ReloadData();
			});
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return _positions.Count;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			string position = _positions[(int)section];
			return _opponents[position].Count;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return _positions[(int)section];
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell<RightDetailTableViewCell>(_cellId, indexPath);

			string position = _positions[indexPath.Section];
			var (player, points) = _opponents[position][indexPath.Row];

			cell.TextLabel.Text = player.FullName;
			cell.DetailTextLabel.Text = points.ToString();

			return cell;
		}
	}
}
