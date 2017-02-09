using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class SettingsController : BaseAuthenticatedApiController
    {
        private readonly ISettingsService service;

        public SettingsController(ISettingsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EmptyApiResponse))]
        [Route("api/settings/close-cases")]
        [HttpPost]
        public async Task<HttpResponseMessage> CloseCases()
        {
            var response = await service.CloseCases();
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EmptyApiResponse))]
        [Route("api/settings/delete-channels")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteChannels()
        {
            var response = await service.DeleteChannels();
            return SendHttpResponse(response);
        }
    }
}
