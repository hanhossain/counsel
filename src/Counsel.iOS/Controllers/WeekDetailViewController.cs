using System;
using System.Collections.Generic;
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

		private List<Player> _opponents = new List<Player>();

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
			Title = $"{_player.FullName} - Week: {_week}";
			TableView.RegisterClassForCellReuse<SubtitleTableViewCell>(_cellId);

			_opponents = await _fantasyService.GetOpponentsAsync(_player.Id, _playerStats.Season, _week);

			InvokeOnMainThread(() => TableView.ReloadData());
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return _opponents.Count;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return "Opponents";
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell<SubtitleTableViewCell>(_cellId, indexPath);

			var opponent = _opponents[indexPath.Row];
			cell.TextLabel.Text = opponent.FullName;
			cell.DetailTextLabel.Text = $"{opponent.Position} - {opponent.Team}";
			return cell;
		}
	}
}
