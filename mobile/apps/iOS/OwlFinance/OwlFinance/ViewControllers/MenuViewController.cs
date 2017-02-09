using System;
using System.Collections.Specialized;
using Foundation;
using OwlFinance.DataSources;
using OwlFinance.Helpers;
using OwlFinance.Managers;
using OwlFinance.ViewModels;
using Twilio.OwlFinance.Domain.Model.ServiceModel;
using UIKit;
using Xamarin.SWRevealViewController;

namespace OwlFinance.ViewControllers
{
	public partial class MenuViewController : BaseUIViewController
    {
		private readonly MenuViewModel viewModel = new MenuViewModel();

		private MenuListDataSource dataSource;
	
        public MenuViewController (IntPtr handle) 
			: base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			SetupBackground();

            viewModel.MenuItems.CollectionChanged += MenuItems_CollectionChanged;
			viewModel.FetchMenuCommand();
		}

		private void SetupBackground()
		{
			View.BackgroundColor = Colors.DarkBlueColor;
			View.Opaque = false;
            MenuTableView.BackgroundColor = UIColor.Clear;
			MenuTableView.Opaque = false;
		}

		private void MenuItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			dataSource = new MenuListDataSource(viewModel.MenuItems);
			dataSource.DidSelectMenuItem += DataSource_DidSelectMenuItem;
			MenuTableView.Source = dataSource;
			MenuTableView.ReloadData();

			if (viewModel.MenuItems.Count > 0)
			{
				View.BringSubviewToFront(MenuTableView);
			}
		}

		private void DataSource_DidSelectMenuItem(MenuServiceModel menu)
		{
			if (menu.MenuItemName == "LOGOUT")
			{
				AppSettingsManager.AccountId = "";
				AppSettingsManager.AuthToken = "";

				var loginController = GetViewController(MainStoryboard, "LoginController") as LoginViewController;
				this.RevealViewController().RevealToggleAnimated(true);
				this.RevealViewController().PushFrontViewController(loginController, false);
			}
			if (menu.MenuItemName == "HOME")
			{
				PerformSegue("homesegue", this);
			}
			if (menu.MenuItemName == "MESSAGES")
			{
				PerformSegue("messagessegue", this);
			}
			if (menu.MenuItemName == "SETTINGS")
			{
				PerformSegue("settingssegue", this);
			}
			if (menu.MenuItemName == "HELP")
			{
				PerformSegue("helpsegue", this);
			}
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			var segueReveal = segue as SWRevealViewControllerSegueSetController;
			if (segueReveal == null) return;
 
			this.RevealViewController().PushFrontViewController(segueReveal.DestinationViewController, true);
		}
    }
}