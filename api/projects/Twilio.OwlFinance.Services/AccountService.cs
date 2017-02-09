using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class AccountService : BaseService, IAccountService, IDisposable
    {
        private readonly IAppSettingsProvider appSettings;
        private readonly IRepository<OwlFinanceAccount> accountRepository;
        private readonly IRepository<Case> caseRepository;
        private readonly IRepository<User> userRepository;

        public AccountService(
            IRepository<OwlFinanceAccount> accountRepository, IRepository<Case> caseRepository, IAppSettingsProvider appSettings, ILogger logger, IRepository<User> userRepository)
            : base(logger)
        {
            this.accountRepository = accountRepository;
            this.caseRepository = caseRepository;
            this.appSettings = appSettings;
            this.userRepository = userRepository;
        }

        public async Task<ApiResponse<AccountModel>> GetAccount(int ownerID, int accountID)
        {
            try
            {
                var account = accountRepository.Query()
                    .Where(acct => acct.ID == accountID)
                    .Where(acct => acct.Owner.ID == ownerID)
                    .Select(acct => new {
                        Number = acct.Number,
                        AccountType = acct.AccountType,
                        Balance = acct.Balance,
                        OwnerFirstName = acct.Owner.FirstName,
                        OwnerLastName = acct.Owner.LastName,
                        //OwnerAddress = acct.Owner.Address,
                        CustomerValue = acct.Owner.ValueLevel.ToString(),
                        CreatedDate = acct.CreatedDate
                    })
                    .AsEnumerable()
                    .Select(acct => new AccountModel {
                        AccountNumber = "XXXX-XXXX-XXXX-" + acct.Number.Remove(0, 12),
                        AccountType = acct.AccountType.ToString(),
                        AvailableBalance = acct.Balance / 100m,
                        OwnerName = $"{acct.OwnerFirstName} {acct.OwnerLastName}",
                        //OwnerAddress = acct.OwnerAddress,
                        CustomerValue = acct.CustomerValue,
                        LastStatementDate = null, //TODO: ??
                        LastInteractionDate = null, //TODO: ??
                        CustomerSinceDate = acct.CreatedDate.ToString("MMMM d, yyyy")
                    })
                    .SingleOrDefault();
                var response = new ApiResponse<AccountModel>(account);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<AccountModel>>(e);
            }
        }

        public async Task<EnumerableApiResponse<AccountModel>> GetAccounts(int ownerID)
        {
            try
            {
                var accounts = accountRepository
                    .Query(acct => acct.Owner.ID == ownerID)
                    .Select(acct => new {
                        Number = acct.Number,
                        AccountType = acct.AccountType,
                        Balance = acct.Balance,
                        OwnerFirstName = acct.Owner.FirstName,
                        OwnerLastName = acct.Owner.LastName,
                        //OwnerAddress = acct.Owner.Address,
                        CreatedDate = acct.CreatedDate
                    })
                    .AsEnumerable()
                    .Select(acct => new AccountModel {
                        AccountNumber = "XXXX-XXXX-XXXX-" + acct.Number.Remove(0, 12),
                        AccountType = acct.AccountType.ToString(),
                        AvailableBalance = acct.Balance / 100m,
                        OwnerName = $"{acct.OwnerFirstName} {acct.OwnerLastName}",
                        //OwnerAddress = acct.OwnerAddress,
                        CustomerValue = "Gold", //TODO: ??
                        LastStatementDate = null, //TODO: ??
                        LastInteractionDate = null, //TODO: ??
                        CustomerSinceDate = acct.CreatedDate.ToString("MMMM d, yyyy")
                    });
                var response = new EnumerableApiResponse<AccountModel>(accounts);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<AccountModel>>(e);
            }
        }

        public async Task<EnumerableApiResponse<CaseModelLite>> GetAccountCases(int ownerID, int accountID, bool includeClosed = false)
        {
            try
            {
                var baseStorageUrl = appSettings.Get("storage:BaseUrl");
                var defaultImagePath = appSettings.Get("storage:DefaultProfileImage");

                var cases = caseRepository.Query()
                    .Where(_case => _case.Account.ID == accountID)
                    .Where(_case => includeClosed || _case.Status != CaseStatus.Closed)
                    .Select(_case => new CaseModelLite {
                        Id = _case.ID,
                        AccountOwner = _case.Customer.FirstName + " " + _case.Customer.LastName,
                        AccountOwnerImgUrl = _case.Customer.ImagePath == null
                            ? baseStorageUrl + "/images/" + defaultImagePath
                            : baseStorageUrl + "/images/" + _case.Customer.ImagePath,
                        Summary = _case.Summary,
                        OpenedDate = _case.CreatedDate,
                        ClosedDate = _case.Status == CaseStatus.Closed ? _case.ModifiedDate : (DateTime?)null
                    })
                    .ToList();
                var response = new EnumerableApiResponse<CaseModelLite>(cases);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<CaseModelLite>>(e);
            }
        }

        public async Task<EnumerableApiResponse<AccountEventModel>> GetAccountEvents(int ownerID, int accountID)
        {
            try
            {
                var events = accountRepository.Query()
                    .Where(acct => acct.ID == accountID)
                    .SelectMany(acct => acct.Events)
                    .Select(_event => new AccountEventModel {
                        Date = _event.CreatedDate,
                        Summary = _event.Summary
                    })
                    .ToList();
                var response = new EnumerableApiResponse<AccountEventModel>(events);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<AccountEventModel>>(e);
            }
        }


        public async Task<EnumerableApiResponse<StatementModel>> GetAccountStatements(string userID, int accountID)
        {
            try
            {
                var account = accountRepository.Get(accountID);
                var statements = account.Statements
                    .Select(statement => new StatementModel {
                        Date = statement.CreatedDate,
                        Month = statement.StartDate.ToString("MMMM"),
                        StartBalance = statement.StartBalance / 100m,
                        EndBalance = statement.EndBalance / 100m
                    });
                var response = new EnumerableApiResponse<StatementModel>(statements);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<StatementModel>>(e);
            }
        }

        public async Task<EnumerableApiResponse<TransactionModel>> GetAccountTransactions(string userID, int accountID)
        {
            try
            {
                var account = accountRepository.Get(accountID);
                var transactions = account.Transactions
                    .Select(txn => new TransactionModel {
                        Date = txn.EffectiveDate,
                        Description = txn.Description,
                        Amount = txn.Amount / 100m
                    });
                var response = new EnumerableApiResponse<TransactionModel>(transactions);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<TransactionModel>>(e);
            }
        }

        public async Task<ApiResponse<AccountInfoModel>> GetAccountInformation(string userId)
        {
            try
            {
                var user = userRepository.Query().FirstOrDefault(u => u.IdentityID == userId);

                if (user != null)
                {
                    var account = accountRepository.Query()
                        .Where(acct => acct.Owner.ID == user.ID)
                        .Select(acct => new AccountInfoModel
                        {
                            AccountId = acct.ID,
                            AccountNumber = acct.Number
                        }).SingleOrDefault();
                    if (account != null)
                    {
                        var response = new ApiResponse<AccountInfoModel>(account);
                        return response;
                    }
                }
                throw new ArgumentException("Cannot find user");
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<AccountInfoModel>>(e);
            }
        }

        public async Task<ApiResponse<BalanceModel>> GetAccountBalance(int accountID)
        {
            try
            {
                var account = accountRepository.Query()
                        .Where(acct => acct.ID == accountID)
                        .Select(acct => new BalanceModel
                        {
                            Amount = acct.Balance
                        }).SingleOrDefault();

                var response = new ApiResponse<BalanceModel>(account);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<BalanceModel>>(e);
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
            if (!disposed)
            {
                if (disposing)
                {
                    accountRepository.Dispose();
                    caseRepository.Dispose();
                }

                disposed = true;
            }
        }
        #endregion
    }
}
