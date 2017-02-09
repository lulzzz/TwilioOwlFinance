using System;

namespace Twilio.OwlFinance.Domain.Interfaces
{
    public interface ILogger
    {
        void LogMessage(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception ex);
        void TrackUser(string email, string userId);
    }
}
