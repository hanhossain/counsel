using Foundation;

namespace UIKit
{
	public static class TableViewExtensions
	{
		public static void RegisterClassForCellReuse<T>(this UITableView tableView, string reuseIdentifier)
			where T : UITableViewCell
		{
			tableView.RegisterClassForCellReuse(typeof(T), reuseIdentifier);
		}

		public static T DequeueReusableCell<T>(this UITableView tableView, string identifier, NSIndexPath indexPath)
			where T : UITableViewCell
		{
			return (T)tableView.DequeueReusableCell(identifier, indexPath);
		}
	}
}
