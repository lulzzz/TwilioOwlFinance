using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Domain.Interfaces.Http
{
	public interface IMessageRepository
	{
		Task<List<Message>> GetAccountMessages(int accountId);
		Task<TwilioTokenModel> GetToken(string device, string userIdentityId, string nickName, string pictureUrl);
		Task<TwilioTokenModel> GetCallToken(string nickName);
	}
}