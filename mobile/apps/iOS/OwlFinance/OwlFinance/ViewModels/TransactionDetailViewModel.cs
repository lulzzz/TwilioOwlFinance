using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.ServiceModel;

namespace OwlFinance.ViewModels
{
	public class TransactionDetailViewModel : MobileServicesBaseViewModel
	{
		private TransactionServiceModel transaction;

		public TransactionServiceModel Transaction
		{
			get
			{
				return transaction;
			}
			set
			{
				transaction = value;
				OnPropertyChanged();
			}
		}

		public async Task FetchTransactionDetailCommand(int transactionId, bool showLoadingIndicator = true)
		{
			try
			{
				ShowLoadingIndicator("Loading Transaction", showLoadingIndicator);

				var transactionDetail = await TransactionService.GetTransactionDetail(transactionId);

				Transaction = transactionDetail;

				HideLoadingIndicator(showLoadingIndicator);
			}
			catch (Exception ex)
			{
				LogExceptionAndShowMessage(ex, "Something went wrong with getting transactions, please contact support");
			}
		}
	}
}