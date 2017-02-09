using System;
using OwlFinance.Helpers;
using OwlFinance.Managers;
using OwlFinance.ViewModels;
using OwlFinance.Views.ConversationsControls;
using Twilio.OwlFinance.Domain.Model.ServiceModel;
using UIKit;

namespace OwlFinance.ViewControllers
{
	public partial class TransactionDetailViewController : BaseUIViewController
    {
		private readonly TransactionDetailViewModel viewModel = new TransactionDetailViewModel();
		public TransactionServiceModel SelectedTransaction { get; internal set; }
		public int SelectedTransactionId { get; set; }

		public TransactionDetailViewController (IntPtr handle) 
			: base (handle)
        {
        }

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Layer styles for Card Details View and Amount Details View
			ContactSupportButton.Layer.CornerRadius = 20;
			CardDetailsView.Layer.CornerRadius = 10;
			CardDetailsView.Layer.BorderWidth = 1;
			CardDetailsView.Layer.BorderColor = Colors.GrayColor.CGColor;
			AmountDetailsView.Layer.BorderWidth = 1;
			AmountDetailsView.Layer.BorderColor = Colors.GrayColor.CGColor;

			if (SelectedTransaction != null)
			{
				Title = SelectedTransaction.Summary;

				ContactSupportButton.BackgroundColor = Colors.BarColor;
				viewModel.PropertyChanged += (sender, e) => InvokeOnMainThread(RefreshUI);

				await viewModel.FetchTransactionDetailCommand(SelectedTransaction.TransactionId);
			}
			else
			{
				ContactSupportButton.BackgroundColor = Colors.PureBlueColor;
				viewModel.PropertyChanged += (sender, e) => InvokeOnMainThread(RefreshUI);

				await viewModel.FetchTransactionDetailCommand(SelectedTransactionId);
			}

			ContactSupportButton.TouchUpInside += ContactSupportButton_TouchUpInside;
		}

		public void RefreshUI()
		{
			var cardNumberMask = GenerateCardNumberMask();
			var lastFour = viewModel.Transaction.CardNumber.Substring(11, 4);
			CardNumberLabel.Text = cardNumberMask + lastFour;
			CardExpirationLabel.Text = viewModel.Transaction.ExpirationDisplayDate;
			CardHolderNameLabel.Text = viewModel.Transaction.CardHolderName;
			MerchantNameLabel.Text = viewModel.Transaction.Merchant;
			DateLabel.Text = viewModel.Transaction.DisplayDate;
			DescriptionTextView.Text = viewModel.Transaction.Summary;
			AmountLabel.Text = viewModel.Transaction.DisplayAmount;
			Title = viewModel.Transaction.Summary;
		}

		private void ContactSupportButton_TouchUpInside(object sender, EventArgs e)
		{
			UIAlertController actionSheetAlert = UIAlertController.Create(
				"Dispute Charge", 
				"Select an item from below", 
				UIAlertControllerStyle.ActionSheet);

			// Add Actions
			actionSheetAlert.AddAction(UIAlertAction.Create(
				"Call", 
				UIAlertActionStyle.Default, 
				async (action) =>
				{
					var callview = ConversationsControls.Create();
					await callview.SendCallRequestAsync();	
					await callview.ShowAsync();
				}));

			actionSheetAlert.AddAction(UIAlertAction.Create(
				"Message", 
				UIAlertActionStyle.Default, 
				(action) => NavigateToMessages()));

			actionSheetAlert.AddAction(UIAlertAction.Create(
				"Cancel", 
				UIAlertActionStyle.Cancel, 
				(action) => Console.WriteLine("Cancel button pressed.")));

			// Required for iPad - You must specify a source for the Action Sheet since it is
			// displayed as a popover
			var presentationPopover = actionSheetAlert.PopoverPresentationController;
			if (presentationPopover != null)
			{
				presentationPopover.SourceView = View;
				presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
			}

			// Display the alert
			PresentViewController(actionSheetAlert, true, null);
		}


		private void NavigateToMessages()
		{
			var id = SelectedTransaction == null ? 1 : SelectedTransaction.TransactionId;
			var messagesViewController = GetViewController(MainStoryboard , "MessageDetailViewController") as MessageDetailViewController;
			messagesViewController.SelectedTransactionId = id;
			var navCtrl = new UINavigationController(messagesViewController);
			PresentViewController(navCtrl, true, null);
		}

		private string GenerateCardNumberMask()
		{
			var mask = "";
			for (int i = 1; i <= 12; i++)
			{
				mask += "\u2022";
				if (i % 4 == 0)
				{
					mask += " ";
				}
			}
			return mask;
		}
	}
}