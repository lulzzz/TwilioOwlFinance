using System.Web.Mvc;
using Twilio.OwlFinance.Domain.Interfaces.Settings;

namespace Twilio.OwlFinance.Web.Mvc
{
    public abstract class OwlFinanceViewPage : WebViewPage
    {
        public IAppSettingsProvider Settings { get; set; }
    }
}
