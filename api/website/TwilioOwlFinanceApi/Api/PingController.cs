using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class PingController : BaseApiController
    {
        private readonly IPingService service;

        public PingController(IPingService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Verifies API is up
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<PingModel>))]
        [Route("api/ping/verify")]
        [HttpGet]
        public async Task<HttpResponseMessage> VerifyPing()
        {
            var response = await service.VerifyPing();
            return SendHttpResponse(response);
        }
    }
}
