using System;
using System.Collections.Specialized;
using Foundation;
using OwlFinance.DataSources;
using OwlFinance.ViewModels;
using UIKit;
using Xamarin.SWRevealViewController;

namespace OwlFinance.ViewControllers
{
    public partial class MessagesViewController : UIViewController
    {
		private readonly MessagesViewModel viewModel = new MessagesViewModel();

		private MessagesListDataSource dataSource;

        public MessagesViewController (IntPtr handle) : base (handle)
        {
			Title = "Messages";
        }

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			viewModel.Messages.CollectionChanged += Messages_CollectionChanged;

			await viewModel.FetchMessagesCommand();

			RevealButton.Clicked += (sender, e) => this.RevealViewController().RevealToggleAnimated(true);
			View.AddGestureRecognizer(this.RevealViewController().PanGestureRecognizer);
		}

		private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			dataSource = new MessagesListDataSource(viewModel.Messages);

			MessagesTableView.Source = dataSource;
			MessagesTableView.ReloadData();

			if (viewModel.Messages.Count > 0)
			{
				View.BringSubviewToFront(MessagesTableView);
			}
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier.Equals("messageDetailSegue"))
			{
				var navctrl = segue.DestinationViewController as UINavigationController;
				var vc = navctrl.VisibleViewController as MessageDetailViewController;

				if (vc != null)
				{
					var rowPath = MessagesTableView.IndexPathForSelectedRow;
					var item = viewModel.Messages[rowPath.Row];
					vc.SelectedCaseId = item.Id;
				}
			}

			base.PrepareForSegue(segue, sender);
		}
	}
}