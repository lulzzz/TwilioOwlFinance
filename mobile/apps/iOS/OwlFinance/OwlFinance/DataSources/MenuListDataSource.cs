using System;
using System.Collections.ObjectModel;
using Foundation;
using OwlFinance.Cells;
using OwlFinance.Helpers;
using Twilio.OwlFinance.Domain.Model.ServiceModel;
using UIKit;

namespace OwlFinance.DataSources
{
	public class MenuListDataSource : UITableViewSource
	{
		static readonly NSString CellIdentifier = new NSString("menu_cell");
		private readonly ObservableCollection<MenuServiceModel> menuItems;
		private UIView selectedBackgroundView = new UIView();

		public MenuListDataSource(ObservableCollection<MenuServiceModel> menuItems)
		{
			this.menuItems = menuItems;
			selectedBackgroundView.BackgroundColor = Colors.MidBlueColor;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return menuItems.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(CellIdentifier) as MenuCell;
			var menuItem = menuItems[indexPath.Row];
			cell.SelectedBackgroundView = selectedBackgroundView;
			cell.UpdateData(menuItem);
			return cell;

		}

		public MenuServiceModel GetItem(int id)
		{
			return menuItems[id];
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			DidSelectMenuItem?.Invoke(menuItems[indexPath.Row]);
		}

		public delegate void RowSelectedHandler(MenuServiceModel menu);

		public event RowSelectedHandler DidSelectMenuItem;
	}
}