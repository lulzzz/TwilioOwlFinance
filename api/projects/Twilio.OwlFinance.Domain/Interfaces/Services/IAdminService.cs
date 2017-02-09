using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api.Admin;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
    public interface IAdminService : IDisposable
    {
        Task<ApiResponse<UserModel>> GetUser(int id);
        Task<EnumerableApiResponse<UserModel>> GetUsers();
        Task<EnumerableApiResponse<UserModel>> GetCustomers();
        Task<ApiResponse<UserModel>> SaveUser(UserModel model);
        Task<EmptyApiResponse> DeleteUser(int id);
        Task<ApiResponse<CreateAgentResponse>> CreateAgentAsync(CreateAgentRequest request);
    }
}