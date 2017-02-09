using System;
using Autofac;
using Plugin.Connectivity;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Services;

namespace OwlFinance.ViewModels
{
	public class MobileServicesBaseViewModel : BaseViewModel
	{
		public ITransactionService TransactionService => Application.Container.Resolve<ITransactionService>();
		public IMessageService MessageService => Application.Container.Resolve<IMessageService>();
		public IAccountService AccountService => Application.Container.Resolve<IAccountService>();
		public ISettings Settings => Application.Container.Resolve<ISettings>();
		public ILogger LoggerService => Application.Container.Resolve<ILogger>();

		protected virtual void ShowLoadingIndicator(string message, bool showLoadingIndicator = true)
		{
			if (showLoadingIndicator)
			{
				Acr.UserDialogs.UserDialogs.Instance.ShowLoading(message);
			}
		}

		protected virtual void HideLoadingIndicator(bool showLoadingIndicator = true)
		{
			if (showLoadingIndicator)
			{
				Acr.UserDialogs.UserDialogs.Instance.HideLoading();
			}
		}

		protected virtual void LogExceptionAndShowMessage(Exception ex, string message)
		{
			LoggerService.LogException(ex);
			Acr.UserDialogs.UserDialogs.Instance.ShowError(message);
		}

		protected virtual void ShowErrorMessage(string message)
		{
			Acr.UserDialogs.UserDialogs.Instance.ShowError(message);
		}

		public virtual bool IsConnectedToInternet()
		{
			return CrossConnectivity.Current.IsConnected;
		}
	}
}