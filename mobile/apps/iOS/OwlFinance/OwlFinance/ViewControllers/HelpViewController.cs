using System;
using UIKit;
using Xamarin.SWRevealViewController;

namespace OwlFinance.ViewControllers
{
    public partial class HelpViewController : UIViewController
    {
        public HelpViewController (IntPtr handle) : base (handle)
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
		}
    }
}