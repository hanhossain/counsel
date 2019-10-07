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

		public LineChartView(IEnumerable<ChartEntry> entries)
		{
			_entries = entries ?? throw new ArgumentNullException(nameof(entries));

			var yAxisTick = SetupYAxis();
			SetupXAxis(yAxisTick);
		}

		private void SetupXAxis(UIView yAxisTick)
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
			_xAxisView.CenterYAnchor.ConstraintEqualTo(yAxisTick.CenterYAnchor).Active = true;

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
			for (int i = 1; i < ticks.Length; i++)
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

		private UIView SetupYAxis()
		{
			// setup axis line
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

			// setup axis ticks
			double min = _entries.Min(x => Math.Floor(x.Y));
			if (min > 0)
			{
				min = 0;
			}

			double max = _entries.Max(x => Math.Ceiling(x.Y));
			if (max < 0)
			{
				max = 0;
			}

			double range = max - min;

			int increment = range switch
			{
				double r when r <= 10 => 1,
				double r when r <= 20 => 2,
				_ => 5
			};

			int axisMin = (int)Math.Floor(min / increment);
			int axisMax = (int)Math.Ceiling(max / increment);

			var values = new List<int>();

			for (int i = axisMin; i <= axisMax; i++)
			{
				values.Add(i * increment);
			}

			// if all values are 0, we want to add a few more ticks so the chart doesn't look super weird
			if (axisMin == 0 && axisMax == 0)
			{
				values.AddRange(new int[] { 1, 2, 3, 4, 5 });
			}
			
			var ticks = values.Select(x => new TickView(UILayoutConstraintAxis.Horizontal)
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

			UIView yAxisTick = null;

			// add labels
			for (int i = 0; i < ticks.Length; i++)
			{
				var tick = ticks[i];
				int labelValue = values[^(i + 1)];

				if (labelValue == 0)
				{
					yAxisTick = tick;
				}

				var label = new UILabel()
				{
					Text = labelValue.ToString(),
					Font = UIFont.PreferredCaption2
				};

				AddSubview(label);
				label.CenterYAnchor.ConstraintEqualTo(tick.CenterYAnchor).Active = true;
				label.TrailingAnchor.ConstraintEqualTo(tick.LeadingAnchor, -4).Active = true;
				label.TranslatesAutoresizingMaskIntoConstraints = false;
			}

			return yAxisTick;
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
