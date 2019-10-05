using System;
using Counsel.iOS.Views;
using UIKit;

namespace Counsel.iOS.Controllers
{
	public class SearchViewController : UIViewController
	{
		private readonly ISearchDelegate _searchDelegate;

		private PaddedTextField _playerField;

		public SearchViewController(ISearchDelegate searchDelegate)
		{
			_searchDelegate = searchDelegate;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = "Search";

			View.BackgroundColor = UIColor.SystemBackgroundColor;

			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
			cancelButton.Clicked += CancelButton_Clicked;
			NavigationItem.LeftBarButtonItem = cancelButton;

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			doneButton.Clicked += DoneButton_Clicked;
			NavigationItem.RightBarButtonItem = doneButton;

			var playerLabel = new UILabel()
			{
				Text = "Player name",
				Font = UIFont.PreferredBody
			};
			View.AddSubview(playerLabel);

			_playerField = new PaddedTextField()
			{
				Padding = new UIEdgeInsets(10, 10, 10, 10),
				Font = UIFont.PreferredBody,
				Layer =
				{
					BackgroundColor = UIColor.SecondarySystemBackgroundColor.CGColor,
					CornerRadius = 5
				}
			};
			View.AddSubview(_playerField);

			playerLabel.LeadingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.LeadingAnchor, 16).Active = true;
			playerLabel.BottomAnchor.ConstraintEqualTo(_playerField.TopAnchor, -8).Active = true;

			_playerField.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor).Active = true;
			_playerField.LeadingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.LeadingAnchor, 16).Active = true;
			_playerField.TrailingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TrailingAnchor, -16).Active = true;

			playerLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			_playerField.TranslatesAutoresizingMaskIntoConstraints = false;
		}

		private void CancelButton_Clicked(object sender, EventArgs e)
		{
			DismissViewController(true, null);
		}

		private async void DoneButton_Clicked(object sender, EventArgs e)
		{
			string playerName = _playerField.Text;

			if (!string.IsNullOrWhiteSpace(playerName))
			{
				await _searchDelegate.OnSearchAsync(playerName);
			}

			DismissViewController(true, null);
		}
	}
}
