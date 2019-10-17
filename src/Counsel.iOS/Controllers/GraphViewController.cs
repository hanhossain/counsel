using System.Linq;
using Counsel.Core.Models;
using Counsel.iOS.Graphing;
using UIKit;

namespace Counsel.iOS.Controllers
{
	public class GraphViewController : UIViewController
	{
		private readonly PlayerStats _playerStats;

		public GraphViewController(PlayerStats playerStats)
		{
			_playerStats = playerStats;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = UIColor.SystemBackgroundColor;

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			doneButton.Clicked += DoneButton_Clicked;
			NavigationItem.RightBarButtonItem = doneButton;

			var lineData = new LineData()
			{
				Entries = _playerStats.Weeks.Zip(_playerStats.Points, (week, points) => new ChartEntry()
				{
					X = week,
					Y = points.Points
				}).ToList(),
				Color = UIColor.SystemRedColor
			};

			var projectedLineData = new LineData()
			{
				Entries = _playerStats.Weeks.Zip(_playerStats.Points, (week, points) => new ChartEntry()
				{
					X = week,
					Y = points.Projected
				}).ToList(),
				Color = UIColor.SystemTealColor
			};

			var chartView = new LineChartView(lineData, projectedLineData)
			{
				BackgroundColor = UIColor.Clear
			};

			View.AddSubview(chartView);

			chartView.LeadingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.LeadingAnchor, 16).Active = true;
			chartView.TrailingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TrailingAnchor, -16).Active = true;
			chartView.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor, 16).Active = true;
			chartView.HeightAnchor.ConstraintEqualTo(chartView.WidthAnchor).Active = true;

			chartView.TranslatesAutoresizingMaskIntoConstraints = false;

			var pointsLabelView = new UILabel()
			{
				Text = "Points",
				TextColor = UIColor.SystemRedColor,
				TranslatesAutoresizingMaskIntoConstraints = false
			};

			View.AddSubview(pointsLabelView);

			pointsLabelView.CenterXAnchor.ConstraintEqualTo(chartView.CenterXAnchor).Active = true;
			pointsLabelView.TopAnchor.ConstraintEqualTo(chartView.BottomAnchor, 16).Active = true;


			var projectedPointsLabelView = new UILabel()
			{
				Text = "Projected points",
				TextColor = UIColor.SystemTealColor,
				TranslatesAutoresizingMaskIntoConstraints = false
			};

			View.AddSubview(projectedPointsLabelView);

			projectedPointsLabelView.TopAnchor.ConstraintEqualTo(pointsLabelView.BottomAnchor, 8).Active = true;
			projectedPointsLabelView.CenterXAnchor.ConstraintEqualTo(pointsLabelView.CenterXAnchor).Active = true;
		}

		private void DoneButton_Clicked(object sender, System.EventArgs e)
		{
			DismissViewController(true, null);
		}
	}
}
