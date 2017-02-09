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
    public class TransactionService : ITransactionService
    {
        private readonly ILogger logger;
        private readonly ITransactionRepository transactionRepository;

        public TransactionService(ILogger logger, ITransactionRepository transactionRepository)
        {
            this.logger = logger;
            this.transactionRepository = transactionRepository;
        }

        public async Task<List<TransactionServiceModel>> GetTransactions(string accountNumber)
        {
            try
            {
                var data = await transactionRepository.GetTransactions(accountNumber);

                var transactions = data.ConvertAll<TransactionConverter, Transaction, TransactionServiceModel>();

                return transactions;

            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                return new List<TransactionServiceModel>();
            }
        }

		public async Task<TransactionServiceModel> GetTransactionDetail(int transactionId)
		{
			try
			{
				var data = await transactionRepository.GetTransactionDetail(transactionId);

				var transaction = data.ConvertOne<TransactionConverter, Transaction, TransactionServiceModel>();

				return transaction;

			}
			catch (Exception ex)
			{
				logger.LogException(ex);
				return new TransactionServiceModel();
			}
		}

		public async Task<bool> CreateACase()
		{
			try
			{
				var data = await transactionRepository.CreateACase();

				return data;
			}
			catch (Exception ex)
			{
				logger.LogException(ex);
				return false;
			}
		}

        public async Task<CaseExistenceModel> DoesCaseExist(int transactionId)
        {
            try
            {
                var data = await transactionRepository.DoesCaseExist(transactionId);

                return data;

            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                throw;
            }
        }

        public async Task<CaseCreatedModel> CreateCase(int transactionId)
        {
            try
            {
                var data = await transactionRepository.CreateCase(transactionId);

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
