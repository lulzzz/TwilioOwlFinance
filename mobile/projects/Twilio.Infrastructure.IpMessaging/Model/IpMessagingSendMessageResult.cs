namespace Twilio.Infrastructure.IpMessaging.Model
{
    public class IpMessagingSendMessageResult
    {
        public string Sid { get; set; }
        public string DateCreated { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public int Index { get; set; }
    }
}