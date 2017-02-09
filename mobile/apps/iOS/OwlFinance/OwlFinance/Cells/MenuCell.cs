using System;
using Twilio.OwlFinance.Domain.Model.ServiceModel;
using UIKit;

namespace OwlFinance.Cells
{
    public partial class MenuCell : UITableViewCell
    {
        public MenuCell (IntPtr handle) 
			: base (handle)
        {
			BackgroundColor = UIColor.Clear;
			Opaque = false;
        }

		public void UpdateData(MenuServiceModel model)
		{
			MenuItemLabel.Text = model.MenuItemName;

			if (model.MenuItemName == "HOME")
			{
				MenuItemImageView.Image = UIImage.FromBundle("homeicon");
			}

			if (model.MenuItemName == "ACTIVITY")
			{
				MenuItemImageView.Image = UIImage.FromBundle("activityicon");
			}

			if (model.MenuItemName == "HELP")
			{
				MenuItemImageView.Image = UIImage.FromBundle("helpicon");
			}

			if (model.MenuItemName == "LOGOUT")
			{
				MenuItemImageView.Image = UIImage.FromBundle("logouticon");
			}

			if (model.MenuItemName == "MESSAGES")
			{
				MenuItemImageView.Image = UIImage.FromBundle("messagesicon");
			}

			if (model.MenuItemName == "SETTINGS")
			{
				MenuItemImageView.Image = UIImage.FromBundle("settingsicon");
			}
		}
    }
}