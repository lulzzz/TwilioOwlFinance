using System;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Infrastructure.Communications;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Repositories;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;
using Twilio.OwlFinance.Domain.Model.Data;
using OwlFinanceAccount = Twilio.OwlFinance.Domain.Model.Data.Account;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.Services
{
    public class TransactionService : BaseService, ITransactionService, IDisposable
    {
        private readonly IAppSettingsProvider appSettings;
        private readonly IRepository<Transaction> repository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<OwlFinanceAccount> accountRepository;
        private readonly AccountMatcher accountMatcher;
        private readonly ISmsManager smsManager;
        private readonly IVoiceManager voiceManager;

        public TransactionService(
            IRepository<Transaction> repository,
            IAppSettingsProvider appSettings,
            ILogger logger,
            IRepository<Customer> customerRepository,
            AccountMatcher accountMatcher,
            ISmsManager smsManager,
            IRepository<OwlFinanceAccount> accountRepository, IVoiceManager voiceManager)
            : base(logger)
        {
            this.appSettings = appSettings;
            this.customerRepository = customerRepository;
            this.accountMatcher = accountMatcher;
            this.smsManager = smsManager;
            this.accountRepository = accountRepository;
            this.voiceManager = voiceManager;
            this.repository = repository;
        }

        public async Task<EnumerableApiResponse<TransactionModel>> GetAccountTransactions(string accountNumber)
        {
            try
            {
                accountNumber = (accountNumber ?? string.Empty).Replace("-", "");
                var baseStorageUrl = appSettings.Get("storage:BaseUrl");
                var transactions = repository
                    .Query(t => t.Account.Number == accountNumber)
                    .OfType<Debit>()
                    .Select(txn => new TransactionModel {
                        TransactionId = txn.ID,
                        Date = txn.CreatedDate,
                        Amount = txn.Amount / 100m,
                        Description = txn.Description,
                        CardNumber = txn.PaymentCard.CardNumber,
                        CardExpirationDate = txn.PaymentCard.ExpirationDate,
                        MerchantName = txn.Merchant.Name,
                        MerchantImageUrl = baseStorageUrl + "/images/" + txn.Merchant.ImagePath
                    })
                    .ToList();
                var response = new EnumerableApiResponse<TransactionModel>(transactions);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<TransactionModel>>(e);
            }
        }

        public async Task<ApiResponse<TransactionModel>> GetTransactionDetail(int transactionID)
        {
            try
            {
                var baseStorageUrl = appSettings.Get("storage:BaseUrl");
                var transaction = repository
                    .Query(t => t.ID == transactionID)
                    .OfType<Debit>()
                    .Select(txn => new TransactionModel {
                        TransactionId = txn.ID,
                        Date = txn.CreatedDate,
                        Amount = txn.Amount / 100m,
                        Description = txn.Description,
                        CardNumber = txn.PaymentCard.CardNumber,
                        CardExpirationDate = txn.PaymentCard.ExpirationDate,
                        MerchantName = txn.Merchant.Name,
                        MerchantImageUrl = baseStorageUrl + "/images/" + txn.Merchant.ImagePath,
                        CardHolderName = txn.Account.Owner.FirstName + " " + txn.Account.Owner.LastName
                    })
                    .FirstOrDefault();
                if (transaction == null)
                {
                    throw new ArgumentException("Cannot find transaction");
                }

                var response = new ApiResponse<TransactionModel>(transaction);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<TransactionModel>>(e);
            }
        }

        public async Task<ApiResponse<NotificationModel>> MakeAVoiceCall(string identityID)
        {
            try
            {
                var notificationModel = new NotificationModel
                {
                    IsSuccessful = false
                };

                Customer customer;
                try
                {
                    customer = await accountMatcher.FindPairedCustomerForAgentAsync(identityID);
                }
                catch (Exception e)
                {
                    notificationModel.StatusMessage = e.Message;
                    return new ApiResponse<NotificationModel>(notificationModel);
                }

                if (customer != null)
                {
                    notificationModel = await voiceManager.MakeVoiceCall(customer.PhoneNumber);
                }

                return new ApiResponse<NotificationModel>(notificationModel);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<NotificationModel>>(e);
            }
        }

        public async Task<ApiResponse<NotificationModel>> NotifyUsersOfSuspiciousActivity(string identityID)
        {
            try
            {
                var notificationModel = new NotificationModel {
                    IsSuccessful = false
                };

                Customer customer;
                try
                {
                    customer = await accountMatcher.FindPairedCustomerForAgentAsync(identityID);
                }
                catch (Exception e)
                {
                    notificationModel.StatusMessage = e.Message;
                    return new ApiResponse<NotificationModel>(notificationModel);
                }

                var account = accountRepository
                    .Query(a => a.Owner.ID == customer.ID)
                    .SingleOrDefault();
                var transaction = (await GetAccountTransactions(account?.Number)).Data
                    .FirstOrDefault();
                if (transaction != null)
                {
                    notificationModel = await smsManager
                        .SendSms("Potentially suspicious activity detected. Click here to check it out. ", customer.PhoneNumber, "transactions", transaction.TransactionId);
                }

                return new ApiResponse<NotificationModel>(notificationModel);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<NotificationModel>>(e);
            }
        }

        #region IDisposable Impl
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.repository.Dispose();
                }

                this.disposed = true;
            }
        }
        #endregion
    }
}
