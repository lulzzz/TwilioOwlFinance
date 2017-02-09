using System;
using Foundation;
using OwlFinance.Managers;
using UIKit;
using Xamarin.SWRevealViewController;

namespace OwlFinance.ViewControllers
{
	public partial class SettingsViewController : UIViewController
    {
        public SettingsViewController (IntPtr handle) 
			: base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var revealViewController = this.RevealViewController();

			if (revealViewController != null)
			{
				RevealButton.Clicked += (sender, e) => revealViewController.RevealToggleAnimated(true);
				View.AddGestureRecognizer(revealViewController.PanGestureRecognizer);
			}

			LoggedInEmailLabel.Text = AppSettingsManager.LoggedInUserEmail;
			AccountNumberLabel.Text = AppSettingsManager.AccountNumber;
			VersionNumberLabel.Text = NSBundle.MainBundle.InfoDictionary[
				new NSString("CFBundleShortVersionString")] + 
				"." + 
				NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")];
		}
    }
}