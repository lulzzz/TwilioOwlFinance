using System;
using System.Collections.Specialized;
using Autofac;
using CoreGraphics;
using CoreText;
using Foundation;
using OwlFinance.DataSources;
using OwlFinance.Helpers;
using OwlFinance.Managers;
using OwlFinance.ViewModels;
using OwlFinance.Views.ConversationsControls;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using UIKit;
using Xamarin.SWRevealViewController;

namespace OwlFinance.ViewControllers
{
	public partial class DashboardViewController : BaseUIViewController
	{
		public int SelectedTransactionId { get; set; }

		private ISignalRService SignalRService => Application.Container.Resolve<ISignalRService>();
		private readonly DashboardViewModel viewModel = new DashboardViewModel();
		private TransactionsListDataSource dataSource;
		private UIRefreshControl refreshControl;

		public DashboardViewController(IntPtr handle)
			: base(handle)
		{
			Title = "Checking Account";
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (SelectedTransactionId != 0)
			{
				var messagesViewController = GetViewController(MainStoryboard, "TransactionDetailController") as TransactionDetailViewController;
				messagesViewController.SelectedTransactionId = SelectedTransactionId;
				NavigationController.PushViewController(messagesViewController, true);
			}

			var searchButton = new UIBarButtonItem(UIBarButtonSystemItem.Search);
			searchButton.Clicked += SearchButton_Clicked;

			NavigationItem.SetRightBarButtonItem(searchButton, true);

			viewModel.Transactions.CollectionChanged += Transactions_CollectionChanged;
			viewModel.PropertyChanged += (sender, e) => InvokeOnMainThread(RefreshUI);

			await viewModel.FetchTransactionsCommand();

			var revealViewController = this.RevealViewController();
			if (revealViewController != null)
			{
				RevealButton.Clicked += (sender, e) => revealViewController.RevealToggleAnimated(true);
				View.AddGestureRecognizer(revealViewController.PanGestureRecognizer);
			}
			else 
			{
				// Bug: Currently app needs to be restarted.
				RevealButton.Clicked += async (sender, e) =>
				{
					await UIAlertViewHelpers.ShowAlert(
						"Uh Oh!", 
						"You have to restart the app to see the menu. I promise this wont happen after you restart!", 
						"Got it!");
				};
			}

			LayoutViews();
			SignalRManager.RegisterEventHandlers();
			await SignalRService.ConnectAsync();
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier.Equals("transactiondetailssegue"))
			{
				var vc = segue.DestinationViewController as TransactionDetailViewController;

				if (vc != null)
				{
					var rowPath = TransactionsTableView.IndexPathForSelectedRow;
					var item = viewModel.Transactions[rowPath.Row];
					vc.SelectedTransaction = item;
				}
			}

			NavigationItem.BackBarButtonItem = new UIBarButtonItem(
				string.Empty,
				UIBarButtonItemStyle.Bordered,
				null,
				null);

			base.PrepareForSegue(segue, sender);
		}

		public void RefreshUI()
		{
			BalanceLabel.Text = viewModel.CurrentBalance;
		}

		private async void RefreshControl_ValueChanged(object sender, EventArgs e)
		{
			refreshControl.BeginRefreshing();
			await viewModel.FetchTransactionsCommand();
			refreshControl.EndRefreshing();
		}

		// Check the stuatus of our connections
		private async void SearchButton_Clicked(object sender, EventArgs e) 
		{
			var conversationsPass = "PASS";
			//var conversationsPass = ConversationsControls.Instance.IsListening ? "PASS" : "FAIL";
			var signalrPass = SignalRService.IsConnected ? "PASS" : "FAIL";
			await UIAlertViewHelpers.ShowAlert(
				"Status Check!",
				$"Conversations: {conversationsPass}\n" +
				$"SignalR Events: {signalrPass}\n",
				"Got it!");
		}

		private void Transactions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			dataSource = new TransactionsListDataSource(viewModel.Transactions);

			TransactionsTableView.Source = dataSource;
			TransactionsTableView.ReloadData();

			if (viewModel.Transactions.Count > 0)
			{
				View.BringSubviewToFront(TransactionsTableView);
			}
		}

		// Having a bad day with the designer
		private void LayoutViews()
		{
			// +1 of width due to weird pixel on far right
			var balanceBackground = new UIImageView(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width + 1, 207))
			{
				Image = UIImage.FromBundle("BalanceBackground")
			};
			balanceBackground.ContentMode = UIViewContentMode.ScaleAspectFill;
			View.AddSubview(balanceBackground);
			// Balance Label
			BalanceLabel.Layer.ZPosition = 1;
			BalanceLabel.Center = balanceBackground.Center;

			TransactionsTableView.Center = new CGPoint(View.Center.X, View.Center.Y);

			// Pull to refresh control
			refreshControl = new UIRefreshControl();
			refreshControl.TintColor = UIColor.Gray;
			var attributedString = new NSAttributedString("Getting Latest Transactions",
				new CTStringAttributes()
				{
					ForegroundColorFromContext = true,
					Font = new CTFont("Lato-Regular", 12)
				});

			refreshControl.AttributedTitle = attributedString;
			refreshControl.ValueChanged += RefreshControl_ValueChanged;
			TransactionsTableView.AddSubview(refreshControl);
		}
	}
}