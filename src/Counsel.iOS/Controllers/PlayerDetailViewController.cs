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
		private const string _calculatedDetailCellId = "calculatedDetailCell";
		private const string _rawDetailCellId = "rawDetailCell";

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
			TableView.RegisterClassForCellReuse<RightDetailTableViewCell>(_calculatedDetailCellId);
			TableView.RegisterClassForCellReuse<SubtitleTableViewCell>(_rawDetailCellId);

			Title = _player.FullName;

			var graphButton = new UIBarButtonItem()
			{
				Image = UIImage.FromBundle("Graph")
			};
			graphButton.Clicked += GraphButton_Clicked;
			NavigationItem.RightBarButtonItem = graphButton;

			_playerStats = await _fantasyService.GetStatsAsync(_player.Id);

			InvokeOnMainThread(() =>
			{
				DismissViewController(true, null);
				TableView.ReloadData();
			});
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 2;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (_playerStats == null)
			{
				return 0;
			}

			return (int)section switch
			{
				0 => _playerStats.Weeks.Any() ? 6 : 0,
				1 => _playerStats.Weeks.Count,
				_ => throw new ArgumentException("Invalid section", nameof(section))
			};
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = indexPath.Section == 0
				? tableView.DequeueReusableCell<RightDetailTableViewCell>(_calculatedDetailCellId, indexPath)
				: (UITableViewCell) tableView.DequeueReusableCell<SubtitleTableViewCell>(_rawDetailCellId, indexPath);

			if (_playerStats != null)
			{
				if (indexPath.Section == 0)
				{
					(string label, double stat) = indexPath.Row switch
					{
						0 => ("Mean", _playerStats.Mean),
						1 => ("Median", _playerStats.Median),
						2 => ("Max", _playerStats.Max),
						3 => ("Min", _playerStats.Min),
						4 => ("Range", _playerStats.Range),
						5 => ("Std Dev", _playerStats.PopStdDev),
						_ => throw new ArgumentException("Invalid indexPath", nameof(indexPath))
					};

					cell.TextLabel.Text = label;
					cell.DetailTextLabel.Text = stat.ToString("f2");
				}
				else if (indexPath.Section == 1)
				{
					int week = _playerStats.Weeks[indexPath.Row];
					(double points, double projectedPoints) = _playerStats.Points[indexPath.Row];

					cell.TextLabel.Text = $"{points} ({projectedPoints} projected)";
					cell.DetailTextLabel.Text = $"Week {week}";
				}
			}

			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			// only continue if a 'raw stats' row is selected
			if (indexPath.Section == 1)
			{
				var weekDetailController = new WeekDetailViewController(_fantasyService, _player, _playerStats, _playerStats.Weeks[indexPath.Row]);
				NavigationController.PushViewController(weekDetailController, true);
			}

			tableView.DeselectRow(indexPath, true);
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return (int)section switch
			{
				0 => "Calculated stats",
				1 => "Raw stats",
				_ => throw new ArgumentException("Invalid section", nameof(section))
			};
		}

		private void GraphButton_Clicked(object sender, EventArgs e)
		{
			var graphViewController = new GraphViewController(_playerStats);
			var navController = new UINavigationController(graphViewController);
			PresentViewController(navController, true, null);
		}
	}
}
