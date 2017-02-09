using System;
using System.Collections.ObjectModel;
using Foundation;
using OwlFinance.Cells;
using OwlFinance.Helpers;
using Twilio.OwlFinance.Domain.Model.ServiceModel;
using UIKit;

namespace OwlFinance.DataSources
{
	public class TransactionsListDataSource : UITableViewSource
	{
		static readonly NSString CellIdentifier = new NSString("transaction_cell");
		private readonly ObservableCollection<TransactionServiceModel> transactions;

		public TransactionsListDataSource(ObservableCollection<TransactionServiceModel> transactions)
		{
			this.transactions = transactions;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return transactions.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(CellIdentifier) as TransactionCell;
			var transaction = transactions[indexPath.Row];
			cell.UpdateData(transaction);
			return cell;
		}

		public TransactionServiceModel GetItem(int id)
		{
			return transactions[id];
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
		}
	}
}