using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace Twilio.Infrastructure.IpMessaging.Model.ResponseModels
{
    public class Message
    {
        public string sid { get; set; }
        public string account_sid { get; set; }
        public string service_sid { get; set; }
        public string to { get; set; }
        public string date_created { get; set; }
        public string date_updated { get; set; }
        public bool was_edited { get; set; }
        public string from { get; set; }
        public string body { get; set; }
        public int index { get; set; }
        public string url { get; set; }
    }

    public class ChannelMessagesResponseModel
    {
        public Meta meta { get; set; }
        public List<Message> messages { get; set; }
    }
}
