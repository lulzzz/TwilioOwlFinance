using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace Twilio.Infrastructure.IpMessaging.Model.ResponseModels
{
    public class Channel
    {
        public string sid { get; set; }
        public string account_sid { get; set; }
        public string service_sid { get; set; }
        public string friendly_name { get; set; }
        public string unique_name { get; set; }
        public object attributes { get; set; }
        public string type { get; set; }
        public string date_created { get; set; }
        public string date_updated { get; set; }
        public string created_by { get; set; }
        public string url { get; set; }
        public Links links { get; set; }
    }

    public class ChannelsResponseModel
    {
        public List<Channel> channels { get; set; }
        public Meta meta { get; set; }
    }
}
