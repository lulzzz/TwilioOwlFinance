using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.TwiML;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.BankingService.Api
{
    /// <summary>
    /// This API controller is used to receive callback requests from the Twilio REST API.
    /// Requests should be handled without requiring authorization.
    /// </summary>
    public class TwilioCallbackController : BaseApiController
    {
        private readonly ITwilioApiSettingsProvider settings;

        public TwilioCallbackController(ITwilioApiSettingsProvider settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// 
        /// </summary>
        [Route("api/twilio/task-router/callback")]
        [HttpPost]
        public async Task<HttpResponseMessage> TaskRouterAssignmentCallback(FormDataCollection value)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { }, new MediaTypeHeaderValue("application/json"));
        }

        /// <summary>
        /// 
        /// </summary>
        [Route("api/twilio/events/callback")]
        [HttpPost]
        public async Task<HttpResponseMessage> TaskRouterEventCallback(FormDataCollection value)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { }, new MediaTypeHeaderValue("application/json"));
        }



        [Route("api/twilio/voice/callback")]
        [HttpPost]
        public async Task<HttpResponseMessage> VoiceCallBack(FormDataCollection value)
        {
            var fromPhoneNumber = settings.FromPhoneNumber.Replace("+1", "");
            var toPhoneNumber = value.GetValues("PhoneNumber")[0];
            var response = new TwilioResponse();
            response.Dial(toPhoneNumber, new {@callerId = fromPhoneNumber });

            return Request.CreateResponse(HttpStatusCode.OK, response.Element, new XmlMediaTypeFormatter());
        }
    }
}
