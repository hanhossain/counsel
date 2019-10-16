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

		//public CGPoint Point
		//{
		//	get => Frame.Location;
		//	set
		//	{
		//		Frame = new CGRect(value, new CGSize(Size, Size));
		//		// TODO: does this need to be set needs layout instead?
		//		//SetNeedsDisplay();
		//	}
		//}

		public UIColor Color { get; set; } = UIColor.SystemRedColor;

		public double Size { get; set; }

		public override void Draw(CGRect rect)
		{
			using var context = UIGraphics.GetCurrentContext();

			context.AddCircle(rect.GetMidX(), rect.GetMidY(), rect.Height / 2);
			context.SetLineWidth(0);
			context.SetFillColor(Color.CGColor);
			context.DrawPath(CGPathDrawingMode.Fill);
		}
	}
}
