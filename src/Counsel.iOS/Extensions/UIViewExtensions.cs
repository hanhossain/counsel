using System;

namespace UIKit
{
	public static class UIViewExtensions
	{
		public static UIView SetWidth(this UIView view, nfloat width)
		{
			view.WidthAnchor.ConstraintEqualTo(width).Active = true;

			return view;
		}

		public static UIView SetHeight(this UIView view, nfloat height)
		{
			view.HeightAnchor.ConstraintEqualTo(height).Active = true;

			return view;
		}
	}
}
