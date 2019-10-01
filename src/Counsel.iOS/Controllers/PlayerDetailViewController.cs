using System;
using System.Linq;
using Counsel.Core;
using Counsel.Core.Models;
using Counsel.iOS.Views;
using Foundation;
using UIKit;

namespace Counsel.iOS.Controllers
{
	public class PlayerDetailViewController : UITableViewController
	{
		private const string _cellId = "playerDetailCell";

		private readonly Player _player;
		private readonly IFantasyService _fantasyService;

		private PlayerStats _playerStats;

		public PlayerDetailViewController(Player player, IFantasyService fantasyService)
		{
			_player = player;
			_fantasyService = fantasyService;
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.RegisterClassForCellReuse<SubtitleTableViewCell>(_cellId);

			Title = _player.FullName;

			if (!await _fantasyService.ContainsStatsAsync())
			{
				var loadingAlert = UIAlertController.Create("Loading...", null, UIAlertControllerStyle.Alert);
				BeginInvokeOnMainThread(() => PresentViewController(loadingAlert, true, null));
			}

			_playerStats = await _fantasyService.GetStatsAsync(_player.Id);

			BeginInvokeOnMainThread(() =>
			{
				DismissViewController(true, null);
				TableView.ReloadData();
			});
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return _playerStats?.Weeks.Count() ?? 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell<SubtitleTableViewCell>(_cellId, indexPath);

			if (_playerStats != null)
			{
				int week = _playerStats.Weeks[indexPath.Row];
				double points = _playerStats.Points[indexPath.Row];

				cell.TextLabel.Text = points.ToString();
				cell.DetailTextLabel.Text = $"Week {week}";
			}

			return cell;
		}
	}
}
