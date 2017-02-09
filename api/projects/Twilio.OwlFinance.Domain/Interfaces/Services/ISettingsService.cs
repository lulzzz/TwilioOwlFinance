using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
    public interface ISettingsService : IDisposable
    {
        Task<EmptyApiResponse> CloseCases();
        Task<EmptyApiResponse> DeleteChannels();
    }
}
