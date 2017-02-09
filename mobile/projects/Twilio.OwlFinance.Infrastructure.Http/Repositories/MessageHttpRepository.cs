using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Http;
using Twilio.OwlFinance.Domain.Model.ApiModel;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Infrastructure.Http.Interfaces;

namespace Twilio.OwlFinance.Infrastructure.Http.Repositories
{
	public class MessageHttpRepository : HttpRepsitoryBase, IMessageRepository
	{
		public MessageHttpRepository(ILogger logger, IHttpClient httpClient) 
			: base(logger, httpClient)
		{
		}

		public async Task<List<Message>> GetAccountMessages(int accountId)
		{
			var url = string.Format(OwlFinanceUris.CaseMessages, accountId.ToString());
			var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);
				var deserializedData = DeserializeWebResponse<MessagesApiModel>(jsonString);
				return deserializedData.Data;
			}

			Logger.LogError("Failed to receive success from GetTransactions api call");

			throw new ArgumentException("Failed to get messages");
		}


		public async Task<TwilioTokenModel> GetToken(string device, string userIdentityId, string nickName, string pictureUrl)
		{
			var url = string.Format(OwlFinanceUris.GetIpMessagingToken, device, userIdentityId, nickName, pictureUrl);
			var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);
				var deserializedData = DeserializeWebResponse<TwilioTokenResponse>(jsonString);
				return deserializedData.Data;
			}

			Logger.LogError("Failed to receive success from GetToken api call");

			return new TwilioTokenModel();
		}

		public async Task<TwilioTokenModel> GetCallToken(string nickName)
		{
			var url = string.Format(OwlFinanceUris.GetConversationsToken, nickName);
			var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);
				var deserializedData = DeserializeWebResponse<TwilioTokenResponse>(jsonString);
				return deserializedData.Data;
			}

			Logger.LogError("Failed to receive success from GetCallToken api call");

			return new TwilioTokenModel();
		}
	}
}