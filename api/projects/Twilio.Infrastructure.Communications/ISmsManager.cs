using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.Infrastructure.Communications
{
    public interface ISmsManager
    {
        Task<NotificationModel> SendSms(string message, string to, string hostKey, int hostId);
    }
}