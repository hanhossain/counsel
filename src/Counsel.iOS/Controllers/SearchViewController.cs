using System;
using Counsel.iOS.Views;
using Foundation;
using UIKit;

namespace Counsel.iOS.Controllers
{
	public class SearchViewController : UITableViewController
	{
		private const string _searchCellId = "searchCell";

		private readonly ISearchDelegate _searchDelegate;

		private Func<string> _getPlayerName;

		public SearchViewController(ISearchDelegate searchDelegate)
		{
			_searchDelegate = searchDelegate;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.RegisterClassForCellReuse<TextFieldTableViewCell>(_searchCellId);
			Title = "Search";

			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
			cancelButton.Clicked += CancelButton_Clicked;
			NavigationItem.LeftBarButtonItem = cancelButton;

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			doneButton.Clicked += DoneButton_Clicked;
			NavigationItem.RightBarButtonItem = doneButton;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return "Search";
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return 1;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell<TextFieldTableViewCell>(_searchCellId, indexPath);

			cell.TextField.Placeholder = "Player name";

			_getPlayerName = () => cell.TextField.Text;

			return cell;
		}

		private void CancelButton_Clicked(object sender, EventArgs e)
		{
			DismissViewController(true, null);
		}

		private async void DoneButton_Clicked(object sender, EventArgs e)
		{
			string playerName = _getPlayerName?.Invoke();

			if (!string.IsNullOrWhiteSpace(playerName))
			{
				await _searchDelegate.OnSearchAsync(playerName);
			}

			DismissViewController(true, null);
		}
	}
}
