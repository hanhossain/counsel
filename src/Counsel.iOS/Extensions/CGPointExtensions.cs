using System;
namespace CoreGraphics
{
	public static class CGPointExtensions
	{
		public static bool IsEqualTo(this CGPoint point, CGPoint targetPoint)
		{
			bool xEqual = Math.Abs(point.X - targetPoint.X) < 0.01;
			bool yEqual = Math.Abs(point.Y - targetPoint.Y) < 0.01;
			return xEqual && yEqual;
		}
	}
}
