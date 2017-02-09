using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
    public interface IAccountService : IDisposable
    {
        Task<ApiResponse<AccountModel>> GetAccount(int ownerID, int accountID);
        Task<EnumerableApiResponse<AccountModel>> GetAccounts(int ownerID);
        Task<EnumerableApiResponse<CaseModelLite>> GetAccountCases(int ownerID, int accountID, bool includeClosed);
        Task<EnumerableApiResponse<AccountEventModel>> GetAccountEvents(int ownerID, int accountID);
        Task<EnumerableApiResponse<StatementModel>> GetAccountStatements(string userID, int accountID);
        Task<EnumerableApiResponse<TransactionModel>> GetAccountTransactions(string userID, int accountID);
        Task<ApiResponse<AccountInfoModel>> GetAccountInformation(string userId);
        Task<ApiResponse<BalanceModel>> GetAccountBalance(int accountID);
    }
}
