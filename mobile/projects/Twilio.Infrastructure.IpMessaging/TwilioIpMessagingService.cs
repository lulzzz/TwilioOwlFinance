using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Infrastructure.IpMessaging.Interfaces;
using Twilio.Infrastructure.IpMessaging.Model;
using Twilio.Infrastructure.IpMessaging.Repositories;

namespace Twilio.Infrastructure.IpMessaging
{
    public class TwilioIpMessagingService : IMessagingService
    {
        private readonly TwilioRestApiRepository repository;

        public TwilioIpMessagingService() 
			: this(new TwilioRestApiRepository())
        {
            
        }

        public TwilioIpMessagingService(TwilioRestApiRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<IpMessagingChannel>> GetChannels()
        {
            var channels = new List<IpMessagingChannel>();
            try
            {
                var ipMessagingChannels =  await repository.GetChannels();
                foreach (var ipMessagingChannel in ipMessagingChannels)
                {
                    var channel = new IpMessagingChannel
                    {
                        UniqueName = ipMessagingChannel.unique_name,
                        Sid = ipMessagingChannel.sid
                    };

                    channels.Add(channel);
                }
            }
            catch (Exception)
            {
                //Log error
            }

            return channels;
        }

        public async Task<IpMessagingChannel> GetChannelByUniqueName(string uniqueName)
        {
            try
            {
                var ipMessagingChannel = await repository.GetChannelByUniqueName(uniqueName);

                if (ipMessagingChannel != null)
                {
                    var channel = new IpMessagingChannel
                    {
                        UniqueName = ipMessagingChannel.unique_name,
                        Sid = ipMessagingChannel.sid
                    };

                    return channel;
                }
            }
            catch (Exception)
            {
                //Log error
            }
            return new IpMessagingChannel();
        }

        public async Task<List<IpMessagingChannelMember>> GetMembersInChannel(string channelSid)
        {
            var channelMembers = new List<IpMessagingChannelMember>();
            try
            {
                var ipMessagingChannelMembers = await repository.GetMembersInChannel(channelSid);
                foreach (var ipMessagingChannelMember in ipMessagingChannelMembers)
                {
                    var channelMember = new IpMessagingChannelMember
                    {
                        Identity = ipMessagingChannelMember.identity,
                        Sid = ipMessagingChannelMember.sid
                    };

                    channelMembers.Add(channelMember);
                }
            }
            catch (Exception)
            {
                //Log error
            }

            return channelMembers;
        }

        public async Task<List<IpMessagingMessage>> GetMessagesInChannel(string channelSid)
        {
            var channelMessages = new List<IpMessagingMessage>();
            try
            {
                var ipMessagingChannelMessages = await repository.GetMessagesInChannel(channelSid);
                foreach (var ipMessagingChannelMessage in ipMessagingChannelMessages)
                {
                    var channelMessage = new IpMessagingMessage
                    {
                        Sid = ipMessagingChannelMessage.sid,
                        From = ipMessagingChannelMessage.from,
                        Body = ipMessagingChannelMessage.body,
                        DateCreated = ipMessagingChannelMessage.date_created
                    };

                    channelMessages.Add(channelMessage);
                }
            }
            catch (Exception)
            {
                //Log error
            }

            return channelMessages;
        }

        public async Task<IpMessagingJoinChannelResult> JoinChannel(string channelSid, string identity)
        {
            try
            {
                var joinChannelResponse = await repository.JoinChannel(channelSid, identity);
                var joinChannel = new IpMessagingJoinChannelResult
                {
                    Sid = joinChannelResponse.sid,
                    ChannelSid = joinChannelResponse.channel_sid,
                    Identity = joinChannelResponse.identity
                };
                return joinChannel;
            }
            catch (Exception)
            {
                //Log error
            }

            return new IpMessagingJoinChannelResult();
        }

        public async Task<IpMessagingSendMessageResult> SendMessage(string channelSid, string identity, string message)
        {
            try
            {
                var sendMessageResponse = await repository.SendMessageToChannel(channelSid, identity, message);
                var sendMessage = new IpMessagingSendMessageResult
                {
                    Sid = sendMessageResponse.sid,
                    DateCreated = sendMessageResponse.date_created,
                    From = sendMessageResponse.from,
                    Body = sendMessageResponse.body,
                    Index = sendMessageResponse.index
                };
                return sendMessage;
            }
            catch (Exception)
            {
				//Log error
            }

            return new IpMessagingSendMessageResult();
        }
    }
}