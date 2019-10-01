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
	public class PlayerDetailViewController : UITableViewController
	{
		private const string _cellId = "playerDetailCell";

		private readonly Player _player;
		private readonly IFantasyService _fantasyService;
		private List<(int, double)> _points = new List<(int, double)>();
		private bool _loaded;

		public PlayerDetailViewController(Player player, IFantasyService fantasyService)
		{
			_player = player;
			_fantasyService = fantasyService;
			TableView.RegisterClassForCellReuse<SubtitleTableViewCell>(_cellId);
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = _player.FullName;

			var stats = await _fantasyService.GetStatsAsync(_player.Id);
			if (stats.TryGetValue("pts_std", out var points))
			{
				_points = points;
			}

			_loaded = true;
			BeginInvokeOnMainThread(() => TableView.ReloadData());
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return _points.Any() ? _points.Count : 1;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell<SubtitleTableViewCell>(_cellId, indexPath);

			if (_points.Any())
			{
				(int week, double points) = _points[indexPath.Row];

				cell.TextLabel.Text = points.ToString();
				cell.DetailTextLabel.Text = $"Week {week}";
			}
			else
			{
				cell.TextLabel.Text = _loaded ? "No data" : "Loading...";
			}

			return cell;
		}
	}
}
