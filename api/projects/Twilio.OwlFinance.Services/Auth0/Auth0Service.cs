using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using Twilio.OwlFinance.Domain.Interfaces.Settings;

namespace Twilio.OwlFinance.Services.Auth0
{
    public class Auth0Service
    {
        private readonly string _baseUrl;
        private readonly string _jwt;

        
        public Auth0Service(IAppSettingsProvider appSettings)
        {
            _baseUrl = appSettings.Get("auth0:Domain");
            _jwt = appSettings.Get("auth0:AuthToken");
        }


        public async Task<CreateUserResponse> CreatePasswordUserAsync(string email, string password)
        {
            var request = new RestRequest("api/v2/users", Method.POST);

            var body = new
            {
                connection = "Username-Password-Authentication",
                email,
                name = email,
                password
            };

            request.AddJsonBody(body);
            return await ExecuteAsync<CreateUserResponse>(request);
        }


        public async Task<CreateUserResponse> CreatePasswordlessUserAsync(string email, string phone)
        {
            var request = new RestRequest("api/v2/users", Method.POST);

            var body = new
            {
                connection = "sms",
                email,
                phone_number = phone
            };

            request.AddJsonBody(body);
            return await ExecuteAsync<CreateUserResponse>(request);
        }


        private async Task<T> ExecuteAsync<T>(IRestRequest request) where T : Auth0Response, new()
        {
            request.AddHeader("Authorization", $"Bearer {_jwt}");

            var client = new RestClient
            {
                BaseUrl = new Uri(_baseUrl),
            };

            var response = await client.ExecuteTaskAsync<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                throw new ApplicationException(message, response.ErrorException); ;
            }

            if (response.StatusCode != HttpStatusCode.Created)
            {
                response.Data.InvalidRequest = true;
            }

            return response.Data;
        }
    }
}