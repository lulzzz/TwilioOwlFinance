using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.Infrastructure.Communications
{
    public interface IVoiceManager
    {
        Task<NotificationModel> MakeVoiceCall(string to);
    }
}