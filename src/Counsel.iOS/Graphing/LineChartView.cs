using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using UIKit;

namespace Counsel.iOS.Graphing
{
	public class LineChartView : UIView
	{
		private const int Padding = 16;
		private const int TickSize = 10;

		private UIView _xAxisView;
		private UIView _yAxisView;

		private IEnumerable<ChartEntry> _entries;

		public LineChartView()
		{
			// TODO: This is temporary while I test out mock data
			_entries = Enumerable.Range(1, 10).Select(x => new ChartEntry()
			{
				X = x,
				Y = x
			}).ToList();

			SetupXAxis();
			SetupYAxis();
		}

		private void SetupXAxis()
		{
			// setup axis line
			_xAxisView = new UIView()
			{
				BackgroundColor = UIColor.SystemGrayColor
			};
			AddSubview(_xAxisView);

			_xAxisView.HeightAnchor.ConstraintEqualTo(1).Active = true;
			_xAxisView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, Padding).Active = true;
			_xAxisView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -Padding).Active = true;
			_xAxisView.BottomAnchor.ConstraintEqualTo(BottomAnchor, -Padding).Active = true;

			_xAxisView.TranslatesAutoresizingMaskIntoConstraints = false;

			// setup axis ticks
			int segmentCount = _entries.Max(x => (int)Math.Ceiling(x.X)) + 1;

			var ticks = Enumerable.Range(0, segmentCount).Select(x => new TickView(UILayoutConstraintAxis.Vertical)
			{
				BackgroundColor = UIColor.SystemGrayColor
			}).ToArray();

			var stackView = new UIStackView(ticks)
			{
				Axis = UILayoutConstraintAxis.Horizontal,
				Distribution = UIStackViewDistribution.EqualSpacing
			};
			AddSubview(stackView);

			stackView.HeightAnchor.ConstraintEqualTo(TickSize).Active = true;
			stackView.LeadingAnchor.ConstraintEqualTo(_xAxisView.LeadingAnchor).Active = true;
			stackView.TrailingAnchor.ConstraintEqualTo(_xAxisView.TrailingAnchor).Active = true;
			stackView.CenterYAnchor.ConstraintEqualTo(_xAxisView.CenterYAnchor).Active = true;

			stackView.TranslatesAutoresizingMaskIntoConstraints = false;

			// add labels
			for (int i = 0; i < ticks.Length; i++)
			{
				var tick = ticks[i];
				var label = new UILabel()
				{
					Text = i.ToString(),
					Font = UIFont.PreferredCaption2
				};

				AddSubview(label);
				label.CenterXAnchor.ConstraintEqualTo(tick.CenterXAnchor).Active = true;
				label.TopAnchor.ConstraintEqualTo(tick.BottomAnchor, 4).Active = true;
				label.TranslatesAutoresizingMaskIntoConstraints = false;
			}
		}

		private void SetupYAxis()
		{
			_yAxisView = new UIView()
			{
				BackgroundColor = UIColor.SystemGrayColor
			};

			AddSubview(_yAxisView);

			_yAxisView.WidthAnchor.ConstraintEqualTo(1).Active = true;
			_yAxisView.TopAnchor.ConstraintEqualTo(TopAnchor, Padding).Active = true;
			_yAxisView.BottomAnchor.ConstraintEqualTo(BottomAnchor, -Padding).Active = true;
			_yAxisView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, Padding).Active = true;

			_yAxisView.TranslatesAutoresizingMaskIntoConstraints = false;

			int yMin = _entries.Min(x => (int)Math.Floor(x.Y));
			if (yMin > 0)
			{
				yMin = 0;
			}

			int yMax = _entries.Max(x => (int)Math.Ceiling(x.Y));
			if (yMax < 0)
			{
				yMax = 0;
			}


			// setup axis ticks
			int segmentCount = 11;

			var ticks = Enumerable.Range(0, segmentCount).Select(x => new TickView(UILayoutConstraintAxis.Horizontal)
			{
				BackgroundColor = UIColor.SystemGrayColor
			}).ToArray();

			var stackView = new UIStackView(ticks)
			{
				Axis = UILayoutConstraintAxis.Vertical,
				Distribution = UIStackViewDistribution.EqualSpacing
			};
			AddSubview(stackView);

			stackView.WidthAnchor.ConstraintEqualTo(TickSize).Active = true;
			stackView.TopAnchor.ConstraintEqualTo(_yAxisView.TopAnchor).Active = true;
			stackView.BottomAnchor.ConstraintEqualTo(_yAxisView.BottomAnchor).Active = true;
			stackView.CenterXAnchor.ConstraintEqualTo(_yAxisView.CenterXAnchor).Active = true;

			stackView.TranslatesAutoresizingMaskIntoConstraints = false;

			// add labels
			for (int i = 0; i < ticks.Length; i++)
			{
				var tick = ticks[i];
				var label = new UILabel()
				{
					Text = (ticks.Length - i - 1).ToString(),
					Font = UIFont.PreferredCaption2
				};

				AddSubview(label);
				label.CenterYAnchor.ConstraintEqualTo(tick.CenterYAnchor).Active = true;
				label.TrailingAnchor.ConstraintEqualTo(tick.LeadingAnchor, -4).Active = true;
				label.TranslatesAutoresizingMaskIntoConstraints = false;
			}
		}

		public class TickView : UIView
		{
			private readonly UILayoutConstraintAxis _axis;

			public TickView(UILayoutConstraintAxis axis)
			{
				_axis = axis;
			}

			public override CGSize IntrinsicContentSize => _axis switch
			{
				UILayoutConstraintAxis.Horizontal => new CGSize(TickSize, 1),
				UILayoutConstraintAxis.Vertical => new CGSize(1, TickSize),
				_ => throw new NotImplementedException()
			};
		}
	}
}
