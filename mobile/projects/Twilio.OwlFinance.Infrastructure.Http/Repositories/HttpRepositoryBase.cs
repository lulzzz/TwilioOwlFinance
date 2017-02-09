using System;
using System.Net.Http;
using Newtonsoft.Json;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Infrastructure.Http.Interfaces;

namespace Twilio.OwlFinance.Infrastructure.Http.Repositories
{
	public abstract class HttpRepsitoryBase
    {
        private readonly ILogger logger;
        private readonly IHttpClient httpClient;

        protected HttpClient HttpClient
        {
            get
            {
                if (httpClient != null)
                {
                    return httpClient.GetHttpClient();
                }
				//Shouldn't happen
				throw new ArgumentException("Could not instantiate http client");
			}
        }

        protected ILogger Logger => logger;

        protected HttpRepsitoryBase(ILogger logger, IHttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        protected virtual T DeserializeWebResponse<T>(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return default(T);
        }
    }
}