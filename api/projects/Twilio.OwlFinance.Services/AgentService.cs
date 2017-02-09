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
    public class AgentService : BaseService, IAgentService, IDisposable
    {
        private readonly IAppSettingsProvider appSettings;
        private readonly IRepository<Agent> agentRepository;
        private readonly IRepository<Case> caseRepository;
        private readonly ITaskRouterManager taskRouter;
        private readonly AccountMatcher accountMatcher;


        public AgentService(
            IRepository<Agent> agentRepository,
            IRepository<Case> caseRepository,
            ITaskRouterManager taskRouter,
            IAppSettingsProvider appSettings,
            ILogger logger, AccountMatcher accountMatcher)
            : base(logger)
        {
            this.agentRepository = agentRepository;
            this.caseRepository = caseRepository;
            this.taskRouter = taskRouter;
            this.appSettings = appSettings;
            this.accountMatcher = accountMatcher;
        }

        public async Task<ApiResponse<AgentModel>> GetAgent(string userId)
        {
            try
            {
                var agent = GetAgentEntity(userId);
                var worker = await taskRouter.GetWorkerInfo(agent.SID);
                var model = CreateAgentModel(agent, worker);
                return new ApiResponse<AgentModel>(model);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<AgentModel>>(e);
            }
        }

        public async Task<EnumerableApiResponse<CaseModelLite>> GetCases(string userId)
        {
            try
            {
                var cases = string.IsNullOrWhiteSpace(userId)
                    ? Enumerable.Empty<CaseModelLite>()
                    : caseRepository
                        .Query(e => e.Agent.IdentityID == userId)
                        .Select(e => new {
                            e.ID,
                            Customer = e.Customer.FirstName + " " + e.Customer.LastName,
                            e.Customer.ImagePath,
                            e.Summary,
                            e.Status,
                            e.CreatedDate,
                            e.ModifiedDate
                        })
                        .AsEnumerable()
                        .Select(e => new CaseModelLite {
                            Id = e.ID,
                            OpenedDate = e.CreatedDate,
                            AccountOwner = e.Customer,
                            AccountOwnerImgUrl = GetImageUrl(e.ImagePath),
                            Summary = e.Summary,
                            ClosedDate = e.Status == CaseStatus.Closed ? e.ModifiedDate : (DateTime?)null
                        })
                        .ToList();
                return new EnumerableApiResponse<CaseModelLite>(cases);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<CaseModelLite>>(e);
            }
        }

        public async Task<ApiResponse<AgentModel>> SetAgentOffline(string userId)
        {
            try
            {
                var agent = GetAgentEntity(userId);
                var worker = await taskRouter.SetWorkerOffline(agent.SID);
                var model = CreateAgentModel(agent, worker);
                return new ApiResponse<AgentModel>(model);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<AgentModel>>(e);
            }
        }

        public async Task<ApiResponse<AgentModel>> SetAgentOnline(string userId)
        {
            try
            {
                var agent = GetAgentEntity(userId);
                var worker = await taskRouter.SetWorkerOnline(agent.SID);
                var model = CreateAgentModel(agent, worker);
                return new ApiResponse<AgentModel>(model);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<AgentModel>>(e);
            }
        }

        public async Task<ApiResponse<AgentTaskModel>> AcceptTaskReservation(string userId, string taskSid, string reservationSid)
        {
            try
            {
                var agent = GetAgentEntity(userId);
                var task = await taskRouter.AcceptTaskReservation(agent.SID, taskSid, reservationSid);
                var worker = await taskRouter.GetWorkerInfo(agent.SID);
                await UpdateCaseAgent(task.CaseId, agent);
                var model = CreateAgentTaskModel(agent, worker, task);
                return new ApiResponse<AgentTaskModel>(model);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<AgentTaskModel>>(e);
            }
        }

        public async Task<ApiResponse<AgentTaskModel>> DeclineTaskReservation(string userId, string taskSid, string reservationSid)
        {
            try
            {
                var agent = GetAgentEntity(userId);
                var task = await taskRouter.RejectTaskReservation(agent.SID, taskSid, reservationSid);
                var worker = await taskRouter.GetWorkerInfo(agent.SID);
                await UpdateCaseAgent(task.CaseId, null);
                var model = CreateAgentTaskModel(agent, worker, task);
                return new ApiResponse<AgentTaskModel>(model);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<AgentTaskModel>>(e);
            }
        }


        public async Task<ApiResponse<CustomerModel>> GetAssociatedCustomerAsync(string identityID)
        {
            try
            {
                var customer = await accountMatcher.FindPairedCustomerForAgentAsync(identityID);

                var response = new CustomerModel
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Id = customer.ID,
                    IdentityID = customer.IdentityID
                };

                return new ApiResponse<CustomerModel>(response);
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<string> GetAssociatedCustomerPhoneNumber(string identityID)
        {
            try
            {
                var phoneNumber = string.Empty;

                Customer customer;
                try
                {
                    customer = await accountMatcher.FindPairedCustomerForAgentAsync(identityID);
                }
                    // ReSharper disable once UnusedVariable
                catch (Exception e)
                {
                    return phoneNumber;
                }

                if (customer != null)
                {
                    return customer.PhoneNumber;
                }

                return phoneNumber;
            }
                // ReSharper disable once UnusedVariable
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private async Task UpdateCaseAgent(int caseId, Agent agent)
        {
            var _case = caseRepository.Get(caseId);
            _case.Agent = agent;
            _case.Status = agent == null ? CaseStatus.New : CaseStatus.Assigned;
            _case.ModifiedDate = DateTime.UtcNow;
            await caseRepository.SaveChanges();
        }

        private AgentModel CreateAgentModel(Agent agent, dynamic workerInfo)
        {
            var baseStorageUrl = appSettings.Get("storage:BaseUrl");
            var model = new AgentModel {
                Id = agent.ID,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                ImageUrl = GetImageUrl(agent.ImagePath),
                Sid = agent.SID,
                IsAvailable = workerInfo.IsAvailable,
                IsOffline = workerInfo.IsOffline,
                Skills = workerInfo.Skills
            };
            return model;
        }

        private AgentTaskModel CreateAgentTaskModel(Agent agent, dynamic workerInfo, dynamic taskInfo)
        {
            var model = new AgentTaskModel {
                Id = agent.ID,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                ImageUrl = GetImageUrl(agent.ImagePath),
                Sid = agent.SID,
                IsAvailable = workerInfo.IsAvailable,
                IsOffline = workerInfo.IsOffline,
                Skills = workerInfo.Skills,
                CaseId = taskInfo.CaseId,
                TaskSid = taskInfo.Sid
            };
            return model;
        }

        private string GetImageUrl(string path)
        {
            var storageUrl = appSettings.Get("storage:BaseUrl");
            var defaultImagePath = appSettings.Get("storage:DefaultProfileImage");

            var url = string.IsNullOrWhiteSpace(path)
                ? $"{storageUrl}/images/{defaultImagePath}"
                : $"{storageUrl}/images/{path}";
            return url;
        }

        private Agent GetAgentEntity(string userId)
        {
            var agent = agentRepository
                .Query(a => a.IdentityID == userId)
                .SingleOrDefault();
            return agent;
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
                }

                disposed = true;
            }
        }
        #endregion
    }
}
