using OwlFinance.ViewControllers;
using OwlFinance.Views.ConversationsControls;
using UIKit;
using Xamarin.SWRevealViewController;

namespace OwlFinance.Managers.EventActions
{
	public class CloseCaseEvent : BaseEventAction
	{
		public override void Fire()
		{
			AppDelegate.InvokeOnMainThread(() =>
			{
				// Dismiss chat window if open
				if (AppDelegate.Window.RootViewController is SWRevealViewController)
				{
					var vc = AppDelegate.Window.RootViewController as SWRevealViewController;

					if (vc?.FrontViewController is UINavigationController)
					{
						var nav = (UINavigationController)vc.FrontViewController;
						if (nav.VisibleViewController is MessageDetailViewController)
						{
							nav.DismissViewController(true, null);
						}
					}
				}

				AppSettingsManager.CallToken = string.Empty;

				// Dismiss conversations controls if open
				if (ConversationsControls.Instance.IsVisible)
				{
					ConversationsControls.Instance.Clear();
				}
			});
		}
	}
}