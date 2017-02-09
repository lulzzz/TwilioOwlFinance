using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Http;
using Twilio.OwlFinance.Domain.Model.ApiModel;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Infrastructure.Http.Interfaces;

namespace Twilio.OwlFinance.Infrastructure.Http.Repositories
{
    public class TransactionHttpRepository: HttpRepsitoryBase, ITransactionRepository
    {
        public TransactionHttpRepository(ILogger logger, IHttpClient httpClient) : base(logger, httpClient)
        {
        }

        public async Task<List<Transaction>> GetTransactions(string accountNumber)
        {
			var url = string.Format(OwlFinanceUris.GetTransactions, accountNumber);


			var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;

				string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

				var deserializedData = DeserializeWebResponse<TransactionListApiModel>(jsonString);

				return deserializedData.Data;
			}
			Logger.LogError("Failed to receive success from GetTransactions api call");
			return new List<Transaction>();
        }

		public async Task<bool> CreateACase()
		{
			var url = string.Format(OwlFinanceUris.CreateACase);

			var response = await HttpClient.PostAsync(url, null).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				return true;
			}
			Logger.LogError("Failed to create a case");
			return false;
		}

        public async Task<Transaction> GetTransactionDetail(int transactionId)
		{
			var url = string.Format(OwlFinanceUris.GetTransactionDetail, transactionId);


			var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;

				string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

				var deserializedData = DeserializeWebResponse<TransactionDetailApiModel>(jsonString);

				return deserializedData.Data;
			}
			Logger.LogError("Failed to receive success from GetTransactions api call");
			throw new ArgumentException("Failed to get transaction detail");
		}

        public async Task<CaseExistenceModel> DoesCaseExist(int transactionId)
        {
            var url = string.Format(OwlFinanceUris.DoesCaseExist, transactionId);

            var response = await HttpClient.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

                var deserializedData = DeserializeWebResponse<CaseExistenceApiModel>(jsonString);

                return deserializedData.Data;
            }
            Logger.LogError("Failed to receive success from GetTransactions api call");
            throw new ArgumentException("Failed to get transaction detail");
        }

        public async Task<CaseCreatedModel> CreateCase(int transactionId)
        {
            var url = string.Format(OwlFinanceUris.CreateCase);

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("TransactionId", transactionId.ToString())
            });

            var response = await HttpClient.PostAsync(url, formContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

                var deserializedData = DeserializeWebResponse<CaseCreatedApiModel>(jsonString);

                return deserializedData.Data;
            }
            Logger.LogError("Failed to receive success from Create Case api call");
            throw new ArgumentException("Failed to create a case");
        }
    }
}
