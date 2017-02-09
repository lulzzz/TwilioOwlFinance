using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Domain.Model.ServiceModel;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
	public interface IMessageService
	{
		Task<List<MessageServiceModel>> GetMessages(int accountId);
		Task<TwilioTokenModel> GetToken(string device, string userIdentityId, string nickName, string pictureUrl);
		Task<TwilioTokenModel> GetCallToken(string nickName);
	}
}

