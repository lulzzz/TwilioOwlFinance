using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Twilio.Infrastructure.IpMessaging.Model;
using Twilio.Infrastructure.IpMessaging.Model.ResponseModels;

namespace Twilio.Infrastructure.IpMessaging.Repositories
{
    public class TwilioRestApiRepository : HttpRepsitoryBase
    {
        public async Task<List<Channel>> GetChannels()
        {
            var url = string.Format(TwilioIpMessagingConstants.Uris.GetChannels, TwilioIpMessagingConstants.InstanceSid);

            var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

                var deserializedData = DeserializeWebResponse<ChannelsResponseModel>(jsonString);

                return deserializedData.channels;
            }
            throw new ArgumentException("Could not get channels");
        }

        public async Task<ChannelResponseModel> GetChannelByUniqueName(string uniqueName)
        {
            var url = string.Format(TwilioIpMessagingConstants.Uris.GetChannel, TwilioIpMessagingConstants.InstanceSid, uniqueName);

            var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

                var deserializedData = DeserializeWebResponse<ChannelResponseModel>(jsonString);

                return deserializedData;
            }
            throw new ArgumentException("Could not get channel");
        }

        public async Task<List<Member>> GetMembersInChannel(string channelSid)
        {
            var url = string.Format(TwilioIpMessagingConstants.Uris.GetChannelMembers, TwilioIpMessagingConstants.InstanceSid, channelSid);

            var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

                var deserializedData = DeserializeWebResponse<ChannelMembersResponseModel>(jsonString);

                return deserializedData.members;
            }
            throw new ArgumentException("Could not get channel members");
        }

        public async Task<List<Message>>  GetMessagesInChannel(string channelSid)
        {
            var url = string.Format(TwilioIpMessagingConstants.Uris.GetChannelMessages, TwilioIpMessagingConstants.InstanceSid, channelSid);

            var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

                var deserializedData = DeserializeWebResponse<ChannelMessagesResponseModel>(jsonString);

                return deserializedData.messages;
            }
            throw new ArgumentException("Could not get channel messages");
        }

        public async Task<JoinChannelResponseModel> JoinChannel(string channelSid, string identity)
        {
            var url = string.Format(TwilioIpMessagingConstants.Uris.GetChannelMembers, TwilioIpMessagingConstants.InstanceSid, channelSid);

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Identity", identity)
            });

            var response = await HttpClient.PostAsync(url, formContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

                var deserializedData = DeserializeWebResponse<JoinChannelResponseModel>(jsonString);

                return deserializedData;
            }
            throw new ArgumentException("Could not join channel");
        }

        public async Task<SendMessageResponseModel> SendMessageToChannel(string channelSid, string identity, string message)
        {
            var url = string.Format(TwilioIpMessagingConstants.Uris.GetChannelMessages, TwilioIpMessagingConstants.InstanceSid, channelSid);

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("From", identity),
                new KeyValuePair<string, string>("Body", message)
            });

            var response = await HttpClient.PostAsync(url, formContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

                var deserializedData = DeserializeWebResponse<SendMessageResponseModel>(jsonString);

                return deserializedData;
            }
            throw new ArgumentException("Could not send message to channel");
        }
    }
}