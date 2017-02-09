using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
    public interface ITransactionService : IDisposable
    {
        Task<EnumerableApiResponse<TransactionModel>> GetAccountTransactions(string accountNumber);
        Task<ApiResponse<TransactionModel>> GetTransactionDetail(int transactionID);
        Task<ApiResponse<NotificationModel>> NotifyUsersOfSuspiciousActivity(string identityID);
        Task<ApiResponse<NotificationModel>> MakeAVoiceCall(string identityID);

    }
}
