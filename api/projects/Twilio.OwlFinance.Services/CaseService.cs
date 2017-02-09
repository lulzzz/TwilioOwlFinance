using System;
using System.Linq;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Repositories;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Interfaces.TaskRouter;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;
using Twilio.OwlFinance.Domain.Model.Data;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.Services
{
    public class CaseService : BaseService, ICaseService, IDisposable
    {
        private readonly IAppSettingsProvider appSettings;
        private readonly IRepository<Agent> agentRepository;
        private readonly IRepository<Case> caseRepository;
        private readonly IRepository<Transaction> transactionRepository;
        private readonly ITaskRouterManager taskRouter;

        public CaseService(
            IRepository<Agent> agentRepository,
            IRepository<Case> caseRepository,
            IRepository<Transaction> transactionRepository,
            ITaskRouterManager taskRouter,
            IAppSettingsProvider appSettings,
            ILogger logger)
            : base(logger)
        {
            this.agentRepository = agentRepository;
            this.caseRepository = caseRepository;
            this.transactionRepository = transactionRepository;
            this.taskRouter = taskRouter;
            this.appSettings = appSettings;
        }

        public async Task<ApiResponse<CaseModel>> GetCase(int id)
        {
            try
            {
                var _case = caseRepository.Get(id);
                var baseStorageUrl = appSettings.Get("storage:BaseUrl");
                var defaultImagePath = appSettings.Get("storage:DefaultProfileImage");

                var model = new CaseModel {
                    Id = _case.ID,
                    CustomerId = _case.Customer.ID,
                    AccountId = _case.Account.ID,
                    AccountNumber = "XXXX-XXXX-XXXX-" + _case.Account.Number.Remove(0, 12),
                    AccountOwner = _case.Customer.FirstName + " " + _case.Customer.LastName,
                    AccountOwnerImgUrl = string.IsNullOrWhiteSpace(_case.Customer.ImagePath)
                        ? baseStorageUrl + "/images/" + defaultImagePath
                        : baseStorageUrl + "/images/" + _case.Customer.ImagePath,
                    AccountOwnerAddress = _case.Customer.Address.Line1,
                    AccountOwnerCityState = _case.Customer.Address.City + ", " + _case.Customer.Address.StateProvince,
                    AccountOwnerPostalCode = _case.Customer.Address.PostalCode,
                    AccountOwnerPhone = _case.Customer.PhoneNumber,
                    AccountType = _case?.Account?.AccountType.ToString(),
                    AvailableBalance = (_case?.Account?.Balance ?? 0) / 100m,
                    TxnAmount = $"{((_case?.Transaction?.Amount ?? 0) / -100m):c}",
                    TxnDescription = _case?.Transaction?.Description,
                    LastStatementDate = _case?.Account.Statements.Max(s => s.EndDate),
                    CustomerValue = _case.Customer.ValueLevel.ToString(),
                    LastInteraction = _case.Events.Any()
                        ? _case.Events.Max(e => e.CreatedDate)
                        : (DateTime?)null,
                    CustomerSince = _case.Customer.CreatedDate,
                    Summary = _case.Summary
                };
                var response = new ApiResponse<CaseModel>(model);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<CaseModel>>(e);
            }
        }

        public async Task<ApiResponse<CaseCreatedModel>> GetOrCreateTransactionCase(CasePostModel model)
        {
            try
            {
                var transactionCase = caseRepository
                    .Query()
                    .Where(e => e.Transaction.ID == model.TransactionId)
                    .SingleOrDefault(e => e.Status != CaseStatus.Closed);
                if (transactionCase == null)
                {
                    var transaction = transactionRepository.Get(model.TransactionId);
                    var summary = $"{(model.CaseType == CaseType.Dispute ? "Disputing" : "Question about")} {transaction.Description}";
                    transactionCase = new Case {
                        Customer = transaction.Account.Owner,
                        Account = transaction.Account,
                        Transaction = transaction,
                        Summary = summary,
                        Status = CaseStatus.New,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    caseRepository.Add(transactionCase);

                    await caseRepository.SaveChanges();

                    await taskRouter.GetOrCreateCaseChannel(transactionCase.ID);

                    var caseEvent = new Event {
                        Account = transactionCase.Account,
                        Summary = "Spoke with an Agent",
                        CreatedDate = DateTime.Now
                    };

                    transactionCase.Events.Add(caseEvent);

                    await caseRepository.SaveChanges();

                    var agentSid = agentRepository
                        .Query(a => a.PairedCustomer.ID == transaction.Account.Owner.ID)
                        .Select(a => a.SID)
                        .Single();

                    await taskRouter.CreateTask(transactionCase.ID, agentSid, model.AgentSkill);
                }

                var responseModel = new CaseCreatedModel {
                    Id = transactionCase.ID,
                    IsCreated = true
                };
                var response = new ApiResponse<CaseCreatedModel>(responseModel);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<CaseCreatedModel>>(e);
            }
        }

        public async Task<ApiResponse<CaseExistenceModel>> DoesCaseExistForTransaction(int transactionId)
        {
            try
            {
                var transactionCase = caseRepository
                    .Query()
                    .Where(e => e.Transaction.ID == transactionId)
                    .SingleOrDefault(e => e.Status != CaseStatus.Closed);
                var caseExistenceModel = new CaseExistenceModel();
                if (transactionCase == null)
                {
                    caseExistenceModel.DoesExist = false;
                    caseExistenceModel.Id = 0;
                }
                else
                {
                    caseExistenceModel.DoesExist = true;
                    caseExistenceModel.Id = transactionCase.ID;
                }

                var response = new ApiResponse<CaseExistenceModel>(caseExistenceModel);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<CaseExistenceModel>>(e);
            }
        }

        public async Task<EnumerableApiResponse<CaseMessageModel>> GetCaseMessages(int accountId)
        {
            try
            {
                var cases = caseRepository
                    .Query()
                    .Where(c => c.Account.ID == accountId)
                    .Select(c => new CaseMessageModel {
                        Id = c.ID,
                        Summary = c.Summary,
                        CaseEvents = c.Events
                    })
                    .ToList();
                foreach (var caseMessageModel in cases)
                {
                    if (caseMessageModel.CaseEvents.Any())
                    {
                        var latestCaseEvent = caseMessageModel
                            .CaseEvents
                            .OrderByDescending(e => e.CreatedDate)
                            .FirstOrDefault();
                        caseMessageModel.LastCorrespondence = latestCaseEvent?.CreatedDate ?? DateTime.Now;
                    }
                    else
                    {
                        caseMessageModel.LastCorrespondence = DateTime.Now;
                    }
                }

                var response = new EnumerableApiResponse<CaseMessageModel>(cases);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<CaseMessageModel>>(e);
            }
        }

        public async Task<ApiResponse<CaseClosedModel>> CloseCase(CloseCaseModel model)
        {
            try
            {
                var transactionCase = caseRepository
                    .Query()
                    .SingleOrDefault(e => e.ID == model.CaseID);
                if (transactionCase != null)
                {
                    transactionCase.Status = CaseStatus.Closed;

                    await caseRepository.SaveChanges();
                }

                var responseModel = new CaseClosedModel { IsSuccess = true };
                var response = new ApiResponse<CaseClosedModel>(responseModel);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<CaseClosedModel>>(e);
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
                    agentRepository.Dispose();
                    caseRepository.Dispose();
                    transactionRepository.Dispose();
                }

                disposed = true;
            }
        }
        #endregion
    }
}
