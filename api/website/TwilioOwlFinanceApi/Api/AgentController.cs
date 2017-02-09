using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;
using Twilio.OwlFinance.Domain.Model.TaskRouter;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class AgentController : BaseAuthenticatedApiController
    {
        private readonly IAgentService agentService;

        public AgentController()
        {
            
        }
        public AgentController(IAgentService agentService)
        {
            this.agentService = agentService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<AgentModel>))]
        [Route("api/agent")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAgent()
        {
            var response = await agentService.GetAgent(UserIdentityID);
            return SendHttpResponse(response);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<CustomerModel>))]
        [Route("api/agent/customer")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCustomer()
        {
            var response = await agentService.GetAssociatedCustomerAsync(UserIdentityID);
            return SendHttpResponse(response);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<CaseModel>))]
        [Route("api/agent/cases")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAgentCases()
        {
            var response = await agentService.GetCases(UserIdentityID);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<AgentModel>))]
        [Route("api/agent/activities/go-offline")]
        [HttpPost]
        public async Task<HttpResponseMessage> GoOffline()
        {
            var response = await agentService.SetAgentOffline(UserIdentityID);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<AgentModel>))]
        [Route("api/agent/activities/go-online")]
        [HttpPost]
        public async Task<HttpResponseMessage> GoOnline()
        {
            var response = await agentService.SetAgentOnline(UserIdentityID);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<AgentTaskModel>))]
        [Route("api/agent/activities/accept-task-reservation")]
        [HttpPost]
        public async Task<HttpResponseMessage> AcceptTaskReservation(ReservationPostModel model)
        {
            var response = await agentService
                .AcceptTaskReservation(UserIdentityID, model.TaskSID, model.ReservationSID);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<AgentTaskModel>))]
        [Route("api/agent/activities/decline-task-reservation")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeclineTaskReservation(ReservationPostModel model)
        {
            var response = await agentService
                .DeclineTaskReservation(UserIdentityID, model.TaskSID, model.ReservationSID);
            return SendHttpResponse(response);
        }
    }
}
