using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OwlFinance.Managers;
using Twilio.OwlFinance.Domain.Model.ServiceModel;

namespace OwlFinance.ViewModels
{
	public class MessagesViewModel : MobileServicesBaseViewModel
	{
		public ObservableCollection<MessageServiceModel> Messages = new ObservableCollection<MessageServiceModel>();

		public async Task FetchMessagesCommand(bool showLoadingIndicator = true)
		{
			try
			{
				ShowLoadingIndicator("Loading Messages", showLoadingIndicator);

				var messages = await MessageService.GetMessages(Convert.ToInt32(AppSettingsManager.AccountId));

				Messages.Clear();

				foreach (var message in messages)
				{
					Messages.Add(message);
				}

				HideLoadingIndicator(showLoadingIndicator);
			}
			catch (Exception ex)
			{
				LogExceptionAndShowMessage(ex, "Something went wrong with getting messages, please contact support");
			}
		}
	}
}