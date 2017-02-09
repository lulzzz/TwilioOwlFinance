using System.Net.Http;

namespace Twilio.OwlFinance.Infrastructure.Http.Interfaces
{
    public interface IHttpClient
    {
        HttpClient GetHttpClient();
    }
}