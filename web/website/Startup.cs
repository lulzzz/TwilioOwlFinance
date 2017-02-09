using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly:OwinStartup(typeof(Twilio.OwlFinance.Web.Startup))]
namespace Twilio.OwlFinance.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration
            {
                EnableJSONP = true
            };
#if DEBUG
            hubConfiguration.EnableDetailedErrors = true;
#endif
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(hubConfiguration);
        }
    }
}