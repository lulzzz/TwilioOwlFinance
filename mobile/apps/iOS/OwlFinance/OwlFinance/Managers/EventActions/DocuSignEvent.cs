using UIKit;
using OwlFinance.ViewControllers;

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
					var topController = AppDelegate.Window.RootViewController;

					while (topController.PresentedViewController != null)
					{
						topController = topController.PresentedViewController;
					}

					topController.PresentViewController(nav, true, null);
				}
			});
		}
	}
}
