using UIKit;

namespace OwlFinance.Managers.EventActions
{
	public class BaseEventAction
	{
		public AppDelegate AppDelegate => (AppDelegate)UIApplication.SharedApplication.Delegate;
		public virtual void Fire() { }
		public virtual void Fire(string data) { }
		public virtual void Fire(string id, string data) { }
	}
}