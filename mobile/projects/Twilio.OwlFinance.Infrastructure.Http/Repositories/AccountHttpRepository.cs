using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Http;
using Twilio.OwlFinance.Domain.Model.ApiModel;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Infrastructure.Http.Interfaces;

namespace Twilio.OwlFinance.Infrastructure.Http.Repositories
{
	public class AccountHttpRepository: HttpRepsitoryBase, IAccountRepository
    {
        public AccountHttpRepository(ILogger logger, IHttpClient httpClient) 
			: base(logger, httpClient)
        {
        }

		public async Task<Balance> GetAccountBalance(int accountId)
		{
			var url = string.Format(OwlFinanceUris.GetAccountBalance, accountId.ToString());

			var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;

				string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

				var deserializedData = DeserializeWebResponse<AccountBalanceApiModel>(jsonString);

				return deserializedData.Data;
			}

			Logger.LogError("Failed to receive success from GetAccountBalance api call");
			throw new ArgumentException("Failed to get account information");
		}

		public async Task<AccountInformation> GetAccountInformation(string userId)
		{
			var url = string.Format(OwlFinanceUris.GetAccountInformation, userId);

			var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;

				string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

				var deserializedData = DeserializeWebResponse<AccountInformationApiModel>(jsonString);

				return deserializedData.Data;
			}

			Logger.LogError("Failed to receive success from GetAccountInformation api call");
			throw new ArgumentException("Failed to get account information");
		}
	}
}