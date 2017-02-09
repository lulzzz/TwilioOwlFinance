using System;
using System.Net.Http;
using System.Net.Http.Headers;
using ModernHttpClient;
using Twilio.OwlFinance.Domain;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Infrastructure.Http.Interfaces;

namespace Twilio.OwlFinance.Infrastructure.Http.Clients
{
    public class OwlFinanceHttpClient : IHttpClient
    {
		private readonly ISettings settings;
		public OwlFinanceHttpClient(ISettings settings)
		{
			this.settings = settings;
		}
		public HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient(new NativeMessageHandler())
            {
                BaseAddress = new Uri(OwlFinanceUris.ApiBaseUri)
            };

			httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + settings.AuthToken);

            httpClient.DefaultRequestHeaders.Accept.Clear();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }
}
