using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Api;
using Twilio.OwlFinance.Domain.Model.TaskRouter;

namespace Twilio.OwlFinance.Domain.Interfaces.TaskRouter
{
    public interface ITaskRouterManager
    {
        Task<dynamic> GetWorkerInfo(string workerSid);
        Task<dynamic> SetWorkerOffline(string workerSid);
        Task<dynamic> SetWorkerOnline(string workerSid);
        Task<dynamic> AcceptTaskReservation(string workerSid, string taskSid, string reservationSid);
        Task<dynamic> RejectTaskReservation(string workerSid, string taskSid, string reservationSid);
        Task<dynamic> CreateTask(int caseId, string agentSid, Skill requiredSkill);
        Task<dynamic> GetOrCreateCaseChannel(int caseId);
    }
}
