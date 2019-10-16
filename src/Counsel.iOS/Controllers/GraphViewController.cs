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

			var optionalHeightConstaint = chartView.HeightAnchor.ConstraintEqualTo(chartView.WidthAnchor);
			optionalHeightConstaint.Priority = 750;
			optionalHeightConstaint.Active = true;

			chartView.BottomAnchor.ConstraintLessThanOrEqualTo(View.SafeAreaLayoutGuide.BottomAnchor).Active = true;

			chartView.TranslatesAutoresizingMaskIntoConstraints = false;
		}

		private void DoneButton_Clicked(object sender, System.EventArgs e)
		{
			DismissViewController(true, null);
		}
	}
}
