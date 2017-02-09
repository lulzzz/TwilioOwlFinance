using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Extensions;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Http;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Domain.Model.ServiceModel;
using Twilio.OwlFinance.Services.Adapters;

namespace Twilio.OwlFinance.Services
{
	public class MessageService : IMessageService
	{
		private readonly ILogger logger;
		private readonly IMessageRepository messageRepository;

		public MessageService(ILogger logger, IMessageRepository messageRepository)
		{
			this.logger = logger;
			this.messageRepository = messageRepository;
		}

		public async Task<List<MessageServiceModel>> GetMessages(int accountId)
		{
			try
			{
				var data = await messageRepository.GetAccountMessages(accountId);
				var messages = data.ConvertAll<MessageConverter, Message, MessageServiceModel>();
				return messages;
			}
			catch (Exception ex)
			{
				logger.LogException(ex);
				return new List<MessageServiceModel>();
			}
		}

		public async Task<TwilioTokenModel> GetToken(string device, string userIdentityId, string nickName, string pictureUrl)
		{
			try
			{
				var data = await messageRepository.GetToken(device, userIdentityId, nickName, pictureUrl);
				return data;
			}
			catch (Exception ex)
			{
				logger.LogException(ex);
				return new TwilioTokenModel();
			}
		}

		public async Task<TwilioTokenModel> GetCallToken(string nickName)
		{
			try
			{
				var data = await messageRepository.GetCallToken(nickName);
				return data;

			}
			catch (Exception ex)
			{
				logger.LogException(ex);
				return new TwilioTokenModel();
			}
		}
	}
}