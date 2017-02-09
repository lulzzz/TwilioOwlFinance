using System;
using System.Threading.Tasks;
using Twilio.Infrastructure.IpMessaging;

namespace TwilioIpMessagingWrapper
{
    class Program
    {
        static void Main(string[] args)
        {

            Task.Run(DoOperations).Wait();

        }

        private static async Task DoOperations()
        {
            await GetChannels();
            var channelSid = await GetChannel();
            await GetChannelMembers(channelSid);
            await GetChannelMessages(channelSid);
            await JoinChannel(channelSid, UserIdentityHelpers.GetJonDavis());
            await SendMessage(channelSid, UserIdentityHelpers.GetJonDavis(), "Hello, world 2");
        }

        private static async Task GetChannels()
        {
            Console.WriteLine("Getting Channels...");
            var service = new TwilioIpMessagingService();

            var channels = await service.GetChannels();

            foreach (var channel in channels)
            {
                Console.WriteLine(channel.UniqueName);
            }
        }

        private static async Task<string> GetChannel()
        {
            Console.WriteLine("Getting Channel...");
            var service = new TwilioIpMessagingService();

            var channel = await service.GetChannelByUniqueName("case71");

            Console.WriteLine(channel.Sid);

            return channel.Sid;
        }

        private static async Task GetChannelMembers(string channelSid)
        {
            Console.WriteLine("Getting Channel Members...");
            var service = new TwilioIpMessagingService();

            var channelMembers = await service.GetMembersInChannel(channelSid);

            foreach (var channelMember in channelMembers)
            {
                Console.WriteLine(channelMember.Identity);
            }
        }

        private static async Task GetChannelMessages(string channelSid)
        {
            Console.WriteLine("Getting Channel Messages...");
            var service = new TwilioIpMessagingService();

            var channelMessages = await service.GetMessagesInChannel(channelSid);

            foreach (var channelMessage in channelMessages)
            {
                Console.WriteLine(channelMessage.Body);
            }
        }

        private static async Task JoinChannel(string channelSid, string identity)
        {
            Console.WriteLine("Joining channel...");
            var service = new TwilioIpMessagingService();

            var joinChannelResult = await service.JoinChannel(channelSid, identity);

            Console.WriteLine(joinChannelResult.Identity);
        }

        private static async Task SendMessage(string channelSid, string identity, string message)
        {
            Console.WriteLine("Sending message...");
            var service = new TwilioIpMessagingService();

            var sendMessageResult = await service.SendMessage(channelSid, identity, message);

            Console.WriteLine(sendMessageResult.Body + " " + sendMessageResult.Index);
        }
    }
}
