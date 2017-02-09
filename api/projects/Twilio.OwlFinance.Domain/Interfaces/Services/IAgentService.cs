using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
    public interface IAgentService : IDisposable
    {
        Task<ApiResponse<AgentModel>> GetAgent(string userId);
        Task<EnumerableApiResponse<CaseModelLite>> GetCases(string userId);
        Task<ApiResponse<AgentModel>> SetAgentOffline(string userId);
        Task<ApiResponse<AgentModel>> SetAgentOnline(string userId);
        Task<ApiResponse<AgentTaskModel>> AcceptTaskReservation(string userId, string taskSid, string reservationSid);
        Task<ApiResponse<AgentTaskModel>> DeclineTaskReservation(string userId, string taskSid, string reservationSid);
        Task<string> GetAssociatedCustomerPhoneNumber(string identityID);
        Task<ApiResponse<CustomerModel>> GetAssociatedCustomerAsync(string identityID);
    }
}
