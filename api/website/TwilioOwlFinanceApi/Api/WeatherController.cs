using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class WeatherController : BaseAuthenticatedApiController
    {
        private readonly IWeatherService weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<WeatherModel>))]
        [Route("api/weather")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetWeather(int customerID)
        {
            var response = await weatherService.GetCustomerWeather(customerID);
            return SendHttpResponse(response);
        }
    }
}
