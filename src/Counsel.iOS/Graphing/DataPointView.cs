using System;
using CoreGraphics;
using UIKit;

namespace Counsel.iOS.Graphing
{
	public class DataPointView : UIView
	{
		public DataPointView()
		{
			BackgroundColor = UIColor.Clear;
		}

		public CGColor Color { get; set; } = UIColor.SystemRedColor.CGColor;

		public override void Draw(CGRect rect)
		{
			using var context = UIGraphics.GetCurrentContext();

			context.AddCircle(rect.GetMidX(), rect.GetMidY(), rect.Height / 2);
			context.SetLineWidth(0);
			context.SetFillColor(Color);
			context.DrawPath(CGPathDrawingMode.Fill);
		}
	}
}
