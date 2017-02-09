using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Domain.Model.ServiceModel;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionServiceModel>> GetTransactions(string accountNumber);
		Task<TransactionServiceModel> GetTransactionDetail(int transactionId);
		Task<bool> CreateACase();
        Task<CaseExistenceModel> DoesCaseExist(int transactionId);
        Task<CaseCreatedModel> CreateCase(int transactionId);
    }
}
