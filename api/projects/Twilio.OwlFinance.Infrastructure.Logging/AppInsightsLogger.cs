using System;
using Twilio.OwlFinance.Domain.Interfaces;

namespace Twilio.OwlFinance.Infrastructure.Logging
{
    public class AppInsightsLogger : ILogger
    {
        public void LogMessage(string message)
        {
            //DO LOGGING
        }

        public void LogWarning(string message)
        {
            //DO LOGGING
        }

        public void LogError(string message)
        {
            //DO LOGGING
        }

        public void LogException(Exception ex)
        {
            //DO LOGGING
        }
    }
}
