namespace Twilio.Infrastructure.IpMessaging.Model
{
    public class TwilioIpMessagingConstants
    {
        public static readonly string InstanceSid = "ISbe1c98c51df04800bc4006ea2dd625cf";
        public static readonly string AccountSid = "AC12243487fa62c3a7e8f9cbaee1cfcb1a";
        public static readonly string AccountAuthToken = "02e3d2ce6faddd7b1fdfcc18a89a8212";
        public class Uris
        {
            public static readonly string BaseApiUri = "https://ip-messaging.twilio.com/v1/";

            public static readonly string GetChannels = "Services/{0}/Channels";
            public static readonly string GetChannel = "Services/{0}/Channels/{1}";
            public static readonly string GetChannelMembers = "Services/{0}/Channels/{1}/Members";
            public static readonly string GetChannelMessages = "Services/{0}/Channels/{1}/Messages";
        }
    }
}
