using CoreGraphics;
using UIKit;

namespace Counsel.iOS.Views
{
	public class PaddedTextField : UITextField
	{
		public UIEdgeInsets Padding { get; set; }

		public override CGRect TextRect(CGRect forBounds)
		{
			return Padding.InsetRect(forBounds);
		}

		public override CGRect PlaceholderRect(CGRect forBounds)
		{
			return Padding.InsetRect(forBounds);
		}

		public override CGRect EditingRect(CGRect forBounds)
		{
			return Padding.InsetRect(forBounds);
		}
	}
}
