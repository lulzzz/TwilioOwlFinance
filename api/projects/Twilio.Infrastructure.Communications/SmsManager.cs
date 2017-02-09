using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Model.Api;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.Infrastructure.Communications
{
    public class SmsManager : ISmsManager
    {
        private readonly ITwilioApiSettingsProvider settings;

        public SmsManager(ITwilioApiSettingsProvider settings)
        {
            this.settings = settings;
        }

        public async Task<NotificationModel> SendSms(string message, string to, string hostKey, int hostId)
        {
            var accountSid = settings.Account.Sid;
            var authToken = settings.AuthToken;
            var fromPhoneNumber = settings.FromPhoneNumber;
            var twilioClient = new TwilioRestClient(accountSid, authToken);
            var status = twilioClient.SendMessage(fromPhoneNumber, to, $"{message} owlfinance://{hostKey}?id={hostId}");
            var model = new NotificationModel { IsSuccessful = true };
            if (status.RestException != null)
            {
                model.IsSuccessful = false;
                model.StatusMessage = status.RestException.Message;
            }

            return model;
        }
    }
}
