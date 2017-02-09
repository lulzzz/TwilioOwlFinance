using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class MerchantsController : BaseAuthenticatedApiController
    {
        private readonly IMerchantService service;

        public MerchantsController(IMerchantService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<MerchantModel>))]
        [Route("api/merchants")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetMerchants()
        {
            var response = await service.GetMerchants();
            return SendHttpResponse(response);
        }
    }
}
