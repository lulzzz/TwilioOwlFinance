namespace Twilio.Infrastructure.IpMessaging.Model
{
    public class TwilioIpMessagingConstants
    {
#if DEBUG
		public static readonly string InstanceSid = "ISa31e77a10d674bbea90eb1b0fbae5b3e";
		public static readonly string AccountSid = "AC843a9ba05cf4835ea1bd10f9558b38eb";
		public static readonly string AccountAuthToken = "55fb778861f35c5942087127d79cf4c6";
#else
        public static readonly string InstanceSid = "IS8b2a27680a4b4d779fadbc1c8f1ac32b";
        public static readonly string AccountSid = "ACc83264f2129bc7560f3b6658e40925f9";
        public static readonly string AccountAuthToken = "cf2c88b6135a89f32d0c69a6f34e61c2";
#endif
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