using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Domain.Interfaces.Http
{
	public interface IAccountRepository
    {
		Task<Balance> GetAccountBalance(int accountId);
		Task<AccountInformation> GetAccountInformation(string userId);
	}
}
