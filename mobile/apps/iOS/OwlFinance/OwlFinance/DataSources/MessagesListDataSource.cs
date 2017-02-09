using System;
using System.Collections.ObjectModel;
using Foundation;
using Twilio.OwlFinance.Domain.Model.ServiceModel;
using UIKit;

namespace OwlFinance.DataSources
{
	public class MessagesListDataSource : UITableViewSource
	{
		static readonly NSString CellIdentifier = new NSString("message_cell");
		private readonly ObservableCollection<MessageServiceModel> messages;

		public MessagesListDataSource(ObservableCollection<MessageServiceModel> messages)
		{
			this.messages = messages;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return messages.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(CellIdentifier);
			var message = messages[indexPath.Row];
			cell.TextLabel.Text = message.Summary;
			cell.DetailTextLabel.Text = message.DisplayLastCorrespondenceDate;
			return cell;
		}

		public MessageServiceModel GetItem(int id)
		{
			return messages[id];
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
		}
	}
}