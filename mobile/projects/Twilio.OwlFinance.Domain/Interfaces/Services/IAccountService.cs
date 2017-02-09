using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
	public interface IAccountService
    {
        Task<string> GetAccountBalance(int accountId);
		Task<AccountInformation> GetAccountInformation(string userId);
	}
}
