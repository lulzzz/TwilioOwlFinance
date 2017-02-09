using System.Web.Optimization;

namespace Twilio.OwlFinance.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterCssBundles(bundles);
            RegisterJavaScriptBundles(bundles);

            //#if DEBUG
            //            BundleTable.EnableOptimizations = false;
            //#else     
            //            BundleTable.EnableOptimizations = true;
            //#endif
            //Setting to false until we figure our bundling

            BundleTable.EnableOptimizations = false; 
        }

        private static void RegisterCssBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/app-css")
                .Include("~/css/bootstrap.min.css")
                .Include("~/css/ie10-viewport-bug-workaround.css")
                .Include("~/css/non-responsive.css")
                .Include("~/css/owl-finance-styles.css")
                .Include("~/css/owl-finance-styles-override.css"));
        }

        private static void RegisterJavaScriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery-js")
                .Include("~/app/js/jquery/jquery-{version}.js")
                .Include("~/app/js/signalr/jquery.signalR-2.2.1.min.js")
                .IncludeDirectory("~/app/js/jquery", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-js")
                .Include("~/app/js/angular/angular.js")
                .IncludeDirectory("~/app/js/angular", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/auth0-js")
                .Include("~/app/js/auth0/lock-9.0.js")
                .Include("~/app/js/auth0/auth0-angular-4.js"));

            bundles.Add(new ScriptBundle("~/bundles/twilio-js")
                .Include("~/app/js/twilio/twilio.min.js")
                .Include("~/app/js/twilio/twilio-common.min.js")
                .Include("~/app/js/twilio/twilio-chat.min.js")
                .Include("~/app/js/twilio/twilio-video.min.js")
                .Include("~/app/js/twilio/taskrouter.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/libs-js")
                .Include("~/app/js/timer/angular-timer.js"));

            bundles.Add(new ScriptBundle("~/bundles/app-js")
                .Include("~/app/app.auth0-variables.js")
                .Include("~/app/app.js")
                .Include("~/app/app.constants.js")
                .Include("~/app/app.config.js")
                .Include("~/app/app.routes.js")
                .IncludeDirectory("~/app/controllers", "*.js", true)
                .IncludeDirectory("~/app/directives", "*.js", true)
                .IncludeDirectory("~/app/services", "*.js", true));
        }
    }
}
