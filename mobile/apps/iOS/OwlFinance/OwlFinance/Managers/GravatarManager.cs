using System;
using System.Collections.Generic;
using UIKit;

namespace OwlFinance.Managers
{
	public enum GravatarUser
	{
		Customer = 0,
		Agent
	};

	public class GravatarManager
	{
		private static Dictionary<GravatarUser, UIImage> gravatarImages = new Dictionary<GravatarUser, UIImage>();

		public static void SetImage(GravatarUser user, string url)
		{
			try
			{
				var image = ImageManager.FromUrl(url);
				gravatarImages.Add(user, image);
			}
			catch (Exception)
			{
				// Ignored
			}
		}

		public static UIImage GetImage(GravatarUser user)
		{
			UIImage image = null;
			gravatarImages.TryGetValue(user, out image);
			return image;
		}
	}
}