using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OwlFinance.Managers;
using Twilio.OwlFinance.Domain.Model.ServiceModel;

namespace OwlFinance.ViewModels
{
	public class DashboardViewModel : MobileServicesBaseViewModel
	{
		private string currentBalance;

		public ObservableCollection<TransactionServiceModel> Transactions = new ObservableCollection<TransactionServiceModel>();
		
		public string CurrentBalance
		{
			get
			{
				return currentBalance;
			}
			set
			{
				currentBalance = value;
				OnPropertyChanged();
			}
		}

		public async Task FetchTransactionsCommand(bool showLoadingIndicator = true)
		{
			try
			{
				ShowLoadingIndicator("Loading Transactions", showLoadingIndicator);

				var transactions = await TransactionService.GetTransactions(AppSettingsManager.AccountNumber);

				Transactions.Clear();

				foreach (var transaction in transactions)
				{
					Transactions.Add(transaction);
				}

				var balance = await AccountService.GetAccountBalance(Convert.ToInt32(AppSettingsManager.AccountId));
				CurrentBalance = balance;

				HideLoadingIndicator(showLoadingIndicator);
			}
			catch (Exception ex)
			{
				LogExceptionAndShowMessage(ex, "Something went wrong with getting transactions, please contact support");
			}
		}
	}
}