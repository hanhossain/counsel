using System;
using System.Net.Http;
using Counsel.iOS.Extensions;
using Counsel.FantasyClient;
using UIKit;

namespace Counsel.iOS
{
    public class ViewController : UIViewController
    {
        private bool _disposed;
        private HttpClient _httpClient;
        private NflFantasyClient _fantasyClient;

        public ViewController()
        {
            _httpClient = new HttpClient();
            _fantasyClient = new NflFantasyClient(_httpClient);
        }

        public override void ViewDidLoad()
        {
            View.BackgroundColor = UIColor.White;

            var button = new UIButton
            {
                BackgroundColor = UIColor.DarkGray
            };


            button.TouchUpInside += Button_TouchUpInside;

            View.AddSubview(button);
            button
                .PinToCenterOf(View)
                .PinSize(100);

            base.ViewDidLoad();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        private async void Button_TouchUpInside(object sender, EventArgs e)
        {
            var result = await _fantasyClient.GetAdvancedResultsAsync(2018, 1);

            foreach (var quarterback in result.Quarterbacks)
            {
                Console.WriteLine($"{quarterback.FirstName} {quarterback.LastName}");
            }
        }
    }
}
