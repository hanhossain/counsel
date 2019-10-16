using CoreGraphics;

namespace Counsel.iOS.Graphing
{
	public class ChartEntry
	{
		public double X { get; set; }
		public double Y { get; set; }

		public CGPoint Point => new CGPoint(X, Y);
	}
}
