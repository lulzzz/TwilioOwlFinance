using OwlFinance.ViewControllers;
using UIKit;
using Xamarin.SWRevealViewController;

namespace OwlFinance.Managers.EventActions
{
	public class DocuSignEvent : BaseEventAction
	{
		public override void Fire(string data)
		{
			AppDelegate.InvokeOnMainThread(() =>
			{
				var vc = AppDelegate.MainStoryboard.InstantiateViewController("DocuSignController") as DocuSignViewController;
				if (vc != null)
				{
					vc.DocuSignUrl = data;
					var nav = new UINavigationController(vc);
					var reveal = AppDelegate.Window.RootViewController as SWRevealViewController;

					// Race condition
					if (reveal == null) return;
					
					var rootNav = reveal.FrontViewController as UINavigationController;
					if (rootNav.VisibleViewController is MessageDetailViewController)
					{
						rootNav
							.PresentedViewController
						   	.PresentViewController(nav, true, null);
					}
					else 
					{
						reveal.PresentViewController(nav, true, null);
					}
				}
			});
		}
	}
}
