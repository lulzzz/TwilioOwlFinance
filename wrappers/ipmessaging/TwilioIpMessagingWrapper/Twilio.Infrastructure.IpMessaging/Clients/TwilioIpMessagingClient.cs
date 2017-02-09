using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Twilio.Infrastructure.IpMessaging.Interfaces;
using Twilio.Infrastructure.IpMessaging.Model;

namespace Twilio.Infrastructure.IpMessaging.Clients
{
    public class TwilioIpMessagingClient : IMessagingClient
    {
        public HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(TwilioIpMessagingConstants.Uris.BaseApiUri)
            };

            var byteArray = Encoding.UTF8.GetBytes(TwilioIpMessagingConstants.AccountSid + ":" + TwilioIpMessagingConstants.AccountAuthToken);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            httpClient.DefaultRequestHeaders.Accept.Clear();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }
}
