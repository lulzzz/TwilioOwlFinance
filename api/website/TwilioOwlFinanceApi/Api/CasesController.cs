using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class CasesController : BaseAuthenticatedApiController
    {
        private readonly ICaseService service;
        private readonly IAgentService agentService;

        public CasesController(ICaseService service, IAgentService agentService)
        {
            this.service = service;
            this.agentService = agentService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<CaseModel>))]
        [Route("api/messages/{accountId:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCaseMessages(int accountId)
        {
            var response = await service.GetCaseMessages(accountId);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<CaseModel>))]
        [Route("api/cases/{id:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get(int id)
        {
            var response = await service.GetCase(id);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<CaseExistenceModel>))]
        [Route("api/cases/{transactionId:int}/doesexist")]
        [HttpGet]
        public async Task<HttpResponseMessage> DoesCaseExistForTransaction(int transactionId)
        {
            var response = await service.DoesCaseExistForTransaction(transactionId);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<CaseCreatedModel>))]
        [Route("api/cases")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateCase(CasePostModel model)
        {
            var response = await service.GetOrCreateTransactionCase(model);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<CaseClosedModel>))]
        [Route("api/closecase")]
        [HttpPost]
        public async Task<HttpResponseMessage> CloseCase(CloseCaseModel model)
        {
            var response = await service.CloseCase(model);
            var setOfflineStatus = await agentService.SetAgentOffline(UserIdentityID);
            var setOnlineStatus = await agentService.SetAgentOnline(UserIdentityID);
            return SendHttpResponse(response);
        }
    }
}
