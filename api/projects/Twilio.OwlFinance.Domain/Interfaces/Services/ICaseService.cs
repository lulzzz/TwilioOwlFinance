using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
    public interface ICaseService : IDisposable
    {
        Task<ApiResponse<CaseModel>> GetCase(int id);
        Task<ApiResponse<CaseCreatedModel>> GetOrCreateTransactionCase(CasePostModel model);
        Task<ApiResponse<CaseExistenceModel>> DoesCaseExistForTransaction(int transactionId);
        Task<EnumerableApiResponse<CaseMessageModel>> GetCaseMessages(int accountId);
        Task<ApiResponse<CaseClosedModel>> CloseCase(CloseCaseModel model);
    }
}
