using Foundation;
using UIKit;

namespace OwlFinance.Managers
{
	public static class ImageManager
	{
		public static UIImage FromUrl(string uri)
		{
			uri = uri.Replace("http://", "https://");

			using (var url = new NSUrl(uri))
			{
				using (var data = NSData.FromUrl(url))
				{
					return UIImage.LoadFromData(data);
				}
			}
		}
	}
}