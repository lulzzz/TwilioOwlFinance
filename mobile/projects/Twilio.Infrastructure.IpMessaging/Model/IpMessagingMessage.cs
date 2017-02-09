namespace Twilio.Infrastructure.IpMessaging.Model
{
    public class IpMessagingMessage
    {
        public string Sid { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string DateCreated { get; set; }
    }
}