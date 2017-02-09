using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Domain.Interfaces.Http
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactions(string accountNumber);
		Task<Transaction> GetTransactionDetail(int transactionId);
		Task<bool> CreateACase();
        Task<CaseExistenceModel> DoesCaseExist(int transactionId);
        Task<CaseCreatedModel> CreateCase(int transactionId);
    }
}
