using Foundation;
using UIKit;

namespace Counsel.iOS.Views
{
	public class SubtitleTableViewCell: UITableViewCell
	{
		[Export("initWithStyle:reuseIdentifier:")]
		public SubtitleTableViewCell(UITableViewCellStyle style, string reuseIdentifier)
			: base(UITableViewCellStyle.Subtitle, reuseIdentifier)
		{
		}
	}
}
