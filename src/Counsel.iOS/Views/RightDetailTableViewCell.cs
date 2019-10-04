using Foundation;
using UIKit;

namespace Counsel.iOS.Views
{
	public class RightDetailTableViewCell : UITableViewCell
	{
		[Export("initWithStyle:reuseIdentifier:")]
		public RightDetailTableViewCell(UITableViewCellStyle style, string reuseIdentifier)
			: base(UITableViewCellStyle.Value1, reuseIdentifier)
		{
		}
	}
}
