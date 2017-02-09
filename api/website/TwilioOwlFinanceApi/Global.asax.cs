using System.Web;
using System.Web.Http;
using Microsoft.ApplicationInsights.Extensibility;

namespace Twilio.OwlFinance.BankingService
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
#if DEBUG
            TelemetryConfiguration.Active.DisableTelemetry = true;
#endif
#if STAGING
             TelemetryConfiguration.Active.DisableTelemetry = true;
#endif
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
