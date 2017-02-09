using System.Net.Http;

namespace Twilio.Infrastructure.IpMessaging.Interfaces
{
    public interface IMessagingClient
    {
        HttpClient GetHttpClient();
    }
}