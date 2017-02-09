using System;

namespace Twilio.OwlFinance.Domain.Interfaces.Settings
{
    public interface IAppSettingsProvider
    {
        string Get(string key);
        T Get<T>(string key) where T : IConvertible;
    }
}
