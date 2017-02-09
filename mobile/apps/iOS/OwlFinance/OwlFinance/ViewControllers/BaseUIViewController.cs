using System;
using Foundation;
using UIKit;

namespace OwlFinance.ViewControllers
{
	public class BaseUIViewController : UIViewController
	{
		public UIStoryboard MainStoryboard => UIStoryboard.FromName("Main", NSBundle.MainBundle);

		public BaseUIViewController(IntPtr handle) 
			: base (handle)
        {
		}

	    public UIViewController GetViewController(UIStoryboard storyboard, string viewControllerName)
		{
			return storyboard.InstantiateViewController(viewControllerName);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			EdgesForExtendedLayout = UIRectEdge.None;
		}

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}
	}
}