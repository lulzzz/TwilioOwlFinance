using System;
using System.Configuration;
using Twilio.OwlFinance.Domain.Interfaces.Settings;

namespace Twilio.OwlFinance.Infrastructure.Settings
{
    public class OwlFinanceSettingsProvider : IAppSettingsProvider
    {
        public string Get(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            return value;
        }

        public T Get<T>(string key) where T : IConvertible
        {
            var value = ConfigurationManager.AppSettings[key];
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
