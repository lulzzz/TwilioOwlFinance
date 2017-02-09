using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Model.Api;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.Infrastructure.Communications
{
    public class VoiceCallManager : IVoiceManager
    {
        private readonly ITwilioApiSettingsProvider settings;

        public VoiceCallManager(ITwilioApiSettingsProvider settings)
        {
            this.settings = settings;
        }

        public async Task<NotificationModel> SendSms(string message, string to, string hostKey, int hostId)
        {
            var fromPhoneNumber = settings.FromPhoneNumber;
            var twilioClient = GetTwilioRestClient();
            var status = twilioClient.SendMessage(fromPhoneNumber, to, $"{message} owlfinance://{hostKey}?id={hostId}");
            var model = new NotificationModel { IsSuccessful = true };
            if (status.RestException != null)
            {
                model.IsSuccessful = false;
                model.StatusMessage = status.RestException.Message;
            }

            return model;
        }

        public async Task<NotificationModel> MakeVoiceCall(string to)
        {
            var fromPhoneNumber = settings.FromPhoneNumber;

            var twilioClient = GetTwilioRestClient();
            var status = twilioClient.InitiateOutboundCall(fromPhoneNumber, to, "https://demo.twilio.com/welcome/voice/");

            var model = new NotificationModel { IsSuccessful = true };

            if (status.RestException != null)
            {
                model.IsSuccessful = false;
                model.StatusMessage = status.RestException.Message;
            }

            return model;

        }

        private TwilioRestClient GetTwilioRestClient()
        {
            var accountSid = settings.Account.Sid;
            var authToken = settings.AuthToken;
            var twilioClient = new TwilioRestClient(accountSid, authToken);
            return twilioClient;
        }
    }
}
