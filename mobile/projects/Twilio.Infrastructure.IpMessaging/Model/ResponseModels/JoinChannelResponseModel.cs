// ReSharper disable InconsistentNaming
namespace Twilio.Infrastructure.IpMessaging.Model.ResponseModels
{
    public class JoinChannelResponseModel
    {
        public string sid { get; set; }
        public string account_sid { get; set; }
        public string channel_sid { get; set; }
        public string service_sid { get; set; }
        public string identity { get; set; }
        public string role_sid { get; set; }
        public object last_consumed_message_index { get; set; }
        public object last_consumption_timestamp { get; set; }
        public string date_created { get; set; }
        public string date_updated { get; set; }
        public string url { get; set; }

    }
}
