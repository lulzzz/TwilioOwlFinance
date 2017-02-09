using System;
using System.Net.Http;
using Newtonsoft.Json;
using Twilio.Infrastructure.IpMessaging.Clients;

namespace Twilio.Infrastructure.IpMessaging.Repositories
{
    public abstract class HttpRepsitoryBase
    {
        private HttpClient httpClient;

        protected HttpClient HttpClient => httpClient ?? (httpClient = new TwilioIpMessagingClient().GetHttpClient());

        protected virtual T DeserializeWebResponse<T>(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
            }
            return default(T);
        }
    }
}