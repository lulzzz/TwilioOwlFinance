using System;
using Twilio.OwlFinance.Domain.Interfaces;

namespace Twilio.OwlFinance.Infrastructure.Logging
{
    public class ApplicationInsightsLogger : ILogger
    {
        public void LogMessage(string message)
        {
            //Do Logging
        }

        public void LogWarning(string message)
        {
            //Do Logging
        }

        public void LogError(string message)
        {
            //Do Logging
        }

        public void LogException(Exception ex)
        {
            //Do Logging
        }

        public void TrackUser(string email, string userId)
        {
            //Do Logging
        }
    }
}
