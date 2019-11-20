using Foundation;
using UIKit;

namespace Counsel.iOS.Views
{
	public class TextFieldTableViewCell : UITableViewCell
	{
		[Export("initWithStyle:reuseIdentifier:")]
		public TextFieldTableViewCell(UITableViewCellStyle style, string reuseIdentifier)
			: base(style, reuseIdentifier)
		{
			TextField = new PaddedTextField()
			{
				Padding = new UIEdgeInsets(8, 16, 8, 16),
				TranslatesAutoresizingMaskIntoConstraints = false
			};

			AddSubview(TextField);

			// TODO: find broken constraint
			TextField.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
			TextField.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
			TextField.LeadingAnchor.ConstraintEqualTo(LeadingAnchor).Active = true;
			TextField.TrailingAnchor.ConstraintEqualTo(TrailingAnchor).Active = true;
			TextField.HeightAnchor.ConstraintEqualTo(46).Active = true;
		}

		public UITextField TextField { get; }
	}
}
