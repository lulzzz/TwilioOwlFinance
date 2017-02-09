using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class TransactionsController : BaseAuthenticatedApiController
    {
        private readonly ITransactionService service;

        public TransactionsController(ITransactionService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<TransactionModel>))]
        [Route("api/accounts/{accountNumber:length(16,19)}/transactions")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTransactions(string accountNumber)
        {
            var response = await service.GetAccountTransactions(accountNumber);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<TransactionModel>))]
        [Route("api/transaction/{transactionID:int}/detail")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTransactionDetail(int transactionID)
        {
            var response = await service.GetTransactionDetail(transactionID);
            return SendHttpResponse(response);
        }
    }
}
