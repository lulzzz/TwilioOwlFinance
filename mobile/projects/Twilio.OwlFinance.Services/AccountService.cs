using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Extensions;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Http;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Services.Adapters;

namespace Twilio.OwlFinance.Services
{
	public class AccountService : IAccountService
	{
		private readonly ILogger logger;
		private readonly IAccountRepository accountRepository;

		public AccountService(ILogger logger, IAccountRepository accountRepository)
		{
			this.logger = logger;
			this.accountRepository = accountRepository;
		}

		public async Task<string> GetAccountBalance(int accountId)
		{
			try
			{
				var data = await accountRepository.GetAccountBalance(accountId);

				var accountBalance = data.ConvertOne<BalanceConverter, Balance, string>();

				return accountBalance;

			}
			catch (Exception ex)
			{
				logger.LogException(ex);
				return string.Empty;
			}
		}

		public async Task<AccountInformation> GetAccountInformation(string userId)
		{
			try
			{
				var data = await accountRepository.GetAccountInformation(userId);

				return data;
			}
			catch (Exception ex)
			{
				logger.LogException(ex);
				throw;
			}
		}


	}
}
