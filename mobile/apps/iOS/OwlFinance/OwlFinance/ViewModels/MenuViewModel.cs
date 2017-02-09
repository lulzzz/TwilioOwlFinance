using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Twilio.OwlFinance.Domain.Model.ServiceModel;

namespace OwlFinance.ViewModels
{
	public class MenuViewModel : MobileServicesBaseViewModel
	{
		public ObservableCollection<MenuServiceModel> MenuItems = new ObservableCollection<MenuServiceModel>();

		public void FetchMenuCommand(bool showLoadingIndicator = true)
		{
			try
			{
				ShowLoadingIndicator("Loading Menu", showLoadingIndicator);

			    var menuItems = new List<MenuServiceModel>
			    {
			        new MenuServiceModel {MenuItemName = "HOME"},
			        new MenuServiceModel {MenuItemName = "ACTIVITY"},
			        new MenuServiceModel {MenuItemName = "MESSAGES"},
			        new MenuServiceModel {MenuItemName = "SETTINGS"},
			        new MenuServiceModel {MenuItemName = "HELP"},
			        new MenuServiceModel {MenuItemName = "LOGOUT"}
			    };

			    MenuItems.Clear();

				foreach (var menuItem in menuItems)
				{
					MenuItems.Add(menuItem);
				}

				HideLoadingIndicator(showLoadingIndicator);
			}
			catch (Exception ex)
			{
				LogExceptionAndShowMessage(ex, "Something went wrong with getting menu, please contact support");
			}
		}
	}
}