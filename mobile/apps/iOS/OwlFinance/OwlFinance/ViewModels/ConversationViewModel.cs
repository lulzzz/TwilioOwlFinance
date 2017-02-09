using System;
using System.Threading.Tasks;

namespace OwlFinance.ViewModels
{
	public class ConversationViewModel : MobileServicesBaseViewModel
	{
		public async Task<string> GetCallToken(string nickName)
		{
			try
			{
				ShowLoadingIndicator("Connecting...");

				var token = await MessageService.GetCallToken(nickName);

				HideLoadingIndicator();

				return token.Token;

			}
			catch (Exception ex)
			{
				LogExceptionAndShowMessage(ex, "Something went wrong with getting token, please contact support");
				return string.Empty;
			}
		}
	}
}