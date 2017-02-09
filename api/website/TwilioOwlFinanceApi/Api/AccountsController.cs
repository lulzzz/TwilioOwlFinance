using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class AccountsController : BaseAuthenticatedApiController
    {
        private readonly IAccountService service;

        public AccountsController(IAccountService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<AccountModel>))]
        [Route("api/accounts")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccounts()
        {
            var user = User;
            var userID = 3; //TODO: ??
            var response = await service.GetAccounts(userID);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<AccountModel>))]
        [Route("api/accounts/{accountID:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccount(int accountID)
        {
            var userID = 3; //TODO: ??
            var response = await service.GetAccount(userID, accountID);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<CaseModelLite>))]
        [Route("api/accounts/{accountID:int}/cases")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccountCases(int accountID, bool includeClosed = true)
        {
            var userID = 3; //TODO: ??
            var response = await service.GetAccountCases(userID, accountID, includeClosed);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<AccountEventModel>))]
        [Route("api/accounts/{accountID:int}/events")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccountEvents(int accountID)
        {
            var userID = 3; //TODO: ??
            var response = await service.GetAccountEvents(userID, accountID);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<StatementModel>))]
        [Route("api/accounts/{accountID:int}/statements")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccountStatements(int accountID)
        {
            var response = await service.GetAccountStatements(UserIdentityID, accountID);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<TransactionModel>))]
        [Route("api/accounts/{accountID:int}/transactions")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccountTransactions(int accountID)
        {
            var response = await service.GetAccountTransactions(UserIdentityID, accountID);
            return SendHttpResponse(response);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<AccountInfoModel>))]
        [Route("api/accounts/{userId}/info")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccountInformation(string userId)
        {
            var response = await service.GetAccountInformation(userId);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<BalanceModel>))]
        [Route("api/accounts/{accountID:int}/balance")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccountBalance(int accountID)
        {
            var response = await service.GetAccountBalance(accountID);
            return SendHttpResponse(response);
        }
    }
}
