using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Counsel.iOS.Graphing
{
	public class LineChartView : UIView
	{
		private const int AxisOffset = 20;
		private const int TickSize = 10;

		private IEnumerable<IEnumerable<(ChartEntry Entry, DataPointView View)>> _entries;
		private IEnumerable<LineData> _lines;

		private readonly UIStringAttributes _axisStringAttributes = new UIStringAttributes()
		{
			ForegroundColor = UIColor.SystemGrayColor,
			Font = UIFont.PreferredCaption2
		};

		public LineChartView(params LineData[] lines)
		{
			_lines = lines ?? throw new ArgumentNullException(nameof(lines));
			_entries = lines
				.Select(line => line.Entries
					.Select(entry => (entry, new DataPointView()
					{
						Size = 10,
						Color = line.Color
					}))
					.ToList())
				.ToList();

			var views = _entries.SelectMany(x => x.Select(y => y.View)).ToArray();
			AddSubviews(views);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			SetNeedsDisplay();
		}

		public override void Draw(CGRect rect)
		{
			// get max, min, and range values
			int xMax = (int)Math.Ceiling(_entries.SelectMany(x => x).Max(x => x.Entry.X));
			int xMin = (int)Math.Floor(_entries.SelectMany(x => x).Min(x => x.Entry.X));
			int xRange = xMax - xMin;

			var xValues = new List<int>();
			for (int x = xMin; x <= xMax; x++)
			{
				xValues.Add(x);
			}

			int yMax = (int)Math.Ceiling(_entries.SelectMany(x => x).Max(x => x.Entry.Y));
			int yMin = (int)Math.Floor(_entries.SelectMany(x => x).Min(x => x.Entry.Y));

			// always show the axis
			if (yMax < 0)
			{
				yMax = 0;
			}

			if (yMin > 0)
			{
				yMin = 0;
			}

			int yRange = yMax - yMin;
			int yStep = yRange switch
			{
				int r when r <= 10 => 1,
				int r when r <= 20 => 2,
				_ => 5
			};

			// update min, max, and range with correct values for yStep
			yMax = yStep * (int)Math.Ceiling(yMax / (double)yStep);
			yMin = yStep * (int)Math.Floor(yMin / (double)yStep);
			yRange = yMax - yMin;

			var yValues = new List<int>();
			for (int y = yMin; y <= yMax; y += yStep)
			{
				yValues.Add(y);
			}

			if (yValues.All(x => x == 0))
			{
				yValues = new List<int>() { 0, 1, 2, 3, 4, 5 };
				yMax = 5;
				yMin = 0;
				yRange = yMax - yMin;
			}

			var chartSize = new CGSize(rect.Width - AxisOffset * 2, rect.Height - AxisOffset * 2);

			var transform = new CGAffineTransform(chartSize.Width / xRange, 0, 0, -chartSize.Height / yRange, AxisOffset, rect.Height - AxisOffset);

			DrawXAxis(xValues, transform, rect);
			DrawYAxis(yValues, transform, rect);

			DrawXLabels(xValues, transform);
			DrawYLabels(yValues, transform);

			UpdateDataPoints(transform);
		}

		private void UpdateDataPoints(CGAffineTransform transform)
		{
			bool shouldUpdateLayout = false;

			// update data points if needed

			foreach (var (entry, view) in _entries.SelectMany(x => x))
			{
				var calculatedPoint = transform.TransformPoint(entry.Point);
				if (!view.Frame.Location.IsEqualTo(calculatedPoint))
				{
					view.Frame = new CGRect(calculatedPoint, new CGSize(10, 10));
					shouldUpdateLayout = true;
				}
			}

			if (shouldUpdateLayout)
			{
				SetNeedsLayout();
			}
		}

		private void DrawXAxis(IEnumerable<int> values, CGAffineTransform transform, CGRect rect)
		{
			using var context = UIGraphics.GetCurrentContext();

			context.AddLines(new[] { new CGPoint(AxisOffset, rect.Height - AxisOffset), new CGPoint(rect.Width - AxisOffset, rect.Height - AxisOffset) });

			var topTransform = CGAffineTransform.MakeTranslation(0, -TickSize / 2);
			var bottomTransform = CGAffineTransform.MakeTranslation(0, TickSize / 2);

			foreach (int x in values)
			{
				// we don't need a tick at the y axis since we have that drawn
				if (x == 0)
				{
					continue;
				}

				var point = new CGPoint(x, 0);
				var transformedPoint = transform.TransformPoint(point);

				var top = topTransform.TransformPoint(transformedPoint);
				var bottom = bottomTransform.TransformPoint(transformedPoint);

				context.AddLines(new[] { top, bottom });
			}

			UIColor.SystemGrayColor.SetStroke();

			context.SetLineWidth(1);
			context.StrokePath();
		}

		private void DrawYAxis(IEnumerable<int> values, CGAffineTransform transform, CGRect rect)
		{
			using var context = UIGraphics.GetCurrentContext();

			context.AddLines(new[] { new CGPoint(AxisOffset, AxisOffset), new CGPoint(AxisOffset, rect.Height - AxisOffset) });

			var leadingTransform = CGAffineTransform.MakeTranslation(-TickSize / 2, 0);
			var trailingTransform = CGAffineTransform.MakeTranslation(TickSize / 2, 0);

			foreach (int y in values)
			{
				// we don't need a tick at the x axis since we have that drawn
				if (y == 0)
				{
					continue;
				}

				var point = new CGPoint(0, y);
				var transformedPoint = transform.TransformPoint(point);

				var leading = leadingTransform.TransformPoint(transformedPoint);
				var trailing = trailingTransform.TransformPoint(transformedPoint);
				context.AddLines(new[] { leading, trailing });
			}

			UIColor.SystemGrayColor.SetStroke();

			context.SetLineWidth(1);
			context.StrokePath();
		}

		private void DrawXLabels(IEnumerable<int> values, CGAffineTransform transform)
		{
			foreach (int x in values)
			{
				// don't draw the 0 label
				if (x == 0)
				{
					continue;
				}

				var label = (NSString)x.ToString();
				var labelSize = label.GetSizeUsingAttributes(_axisStringAttributes);

				var point = new CGPoint(x, 0);
				var transformedPoint = transform.TransformPoint(point);
				var xLabelTransform = CGAffineTransform.MakeTranslation(-labelSize.Width / 2, TickSize / 2 + 3);
				var labelPoint = xLabelTransform.TransformPoint(transformedPoint);

				label.DrawString(labelPoint, _axisStringAttributes);
			}
		}

		private void DrawYLabels(IEnumerable<int> values, CGAffineTransform transform)
		{
			foreach (int y in values)
			{
				// don't draw the 0 label
				if (y == 0)
				{
					continue;
				}

				var label = (NSString)y.ToString();
				var labelSize = label.GetSizeUsingAttributes(_axisStringAttributes);

				var point = new CGPoint(0, y);
				var transformedPoint = transform.TransformPoint(point);
				var labelTransform = CGAffineTransform.MakeTranslation(-TickSize / 2 - labelSize.Width - 3, -labelSize.Height / 2);
				var labelPoint = labelTransform.TransformPoint(transformedPoint);

				label.DrawString(labelPoint, _axisStringAttributes);
			}
		}
	}
}
