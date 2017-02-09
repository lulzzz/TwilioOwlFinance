using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Data;

namespace OwlFinance.ViewModels
{
	public class MessageDetailViewModel : MobileServicesBaseViewModel
	{
		public async Task<string> GetToken(string device, string userIdentityId, string nickName, string pictureUrl)
		{
			try
			{
				var token = await MessageService.GetToken(device, userIdentityId, nickName, pictureUrl);
				return token.Token;

			}
			catch (Exception ex)
			{
				LogExceptionAndShowMessage(ex, "Something went wrong with getting token, please contact support");
				return string.Empty;
			}
		}

	    public async Task<CaseExistenceModel> DoesCaseExist(int transactionId)
	    {
            try
            {
                return await TransactionService.DoesCaseExist(transactionId);
            }
            catch (Exception ex)
            {
                LogExceptionAndShowMessage(ex, "Something went wrong with verifying your case status, please contact support");
                return new CaseExistenceModel();
            }
        }

	    public async Task<CaseCreatedModel> CreateCaseCommand(int transactionId)
	    {
            try
            {
                return await TransactionService.CreateCase(transactionId);

            }
            catch (Exception ex)
            {
                LogExceptionAndShowMessage(ex, "Something went wrong with creating your case status, please contact support");
				throw;
            }
        }
	}
}