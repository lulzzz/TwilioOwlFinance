using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Repositories;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;
using Twilio.OwlFinance.Domain.Model.Data;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.Services
{
    public class WeatherService : BaseService, IWeatherService
    {
        private readonly IAppSettingsProvider settings;
        private readonly IRepository<Customer> repository;

        public WeatherService(IRepository<Customer> repository, IAppSettingsProvider settings, ILogger logger)
            : base(logger)
        {
            this.settings = settings;
            this.repository = repository;
        }

        public async Task<ApiResponse<WeatherModel>> GetCustomerWeather(int customerID)
        {
            try
            {
                var weather = new WeatherModel();
                var customer = this.repository.Get(customerID);

                string forecastPath;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(settings.Get("zipcode:BaseUrl"));

                    var zipCodePath = $"/rest/{settings.Get("zipcode:ApiKey")}/info.json/{customer.Address.PostalCode}/degrees";
                    using (var httpResponse = await client.GetAsync(zipCodePath))
                    {
                        httpResponse.EnsureSuccessStatusCode();

                        var result = await httpResponse.Content.ReadAsStringAsync();
                        dynamic json = JObject.Parse(result);

                        forecastPath = $"/forecast/{settings.Get("weather:ApiKey")}/{json.lat},{json.lng}";
                    }
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(settings.Get("weather:BaseUrl"));
                    using (var httpResponse = await client.GetAsync(forecastPath))
                    {
                        httpResponse.EnsureSuccessStatusCode();

                        var result = await httpResponse.Content.ReadAsStringAsync();
                        dynamic json = JObject.Parse(result);
                        dynamic forecast = json.daily.data[0];
                        weather.ApparentTemp = json.currently.apparentTemperature;
                        weather.CurrentTemp = json.currently.temperature;
                        weather.HighTemp = forecast.temperatureMax;
                        weather.LowTemp = forecast.temperatureMin;
                        weather.PrecipitationPct = forecast.precipProbability * 100;
                    }
                }

                var response = new ApiResponse<WeatherModel>(weather);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<WeatherModel>>(e);
            }
        }
    }
}
