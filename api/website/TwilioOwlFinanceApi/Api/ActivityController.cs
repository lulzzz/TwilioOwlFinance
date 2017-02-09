using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class ActivityController : BaseAuthenticatedApiController
    {
        private readonly ITransactionService transactionService;

        public ActivityController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<NotificationModel>))]
        [Route("api/activity/scan")]
        [HttpPost]
        public async Task<HttpResponseMessage> ScanForActivity()
        {
            var response = await transactionService.NotifyUsersOfSuspiciousActivity(UserIdentityID);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<NotificationModel>))]
        [Route("api/activity/makeavoicecall")]
        [HttpPost]
        public async Task<HttpResponseMessage> MakeAVoiceCall()
        {
            var response = await transactionService.MakeAVoiceCall(UserIdentityID);
            return SendHttpResponse(response);
        }
    }
}