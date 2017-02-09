using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Infrastructure.Settings;

namespace Twilio.OwlFinance.Web.App_Start
{
    public static class AutofacConfig
    {
        public static void Initialize()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule<AutofacWebTypesModule>();

            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterOwlFinanceTypes();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterOwlFinanceTypes(this ContainerBuilder builder)
        {
            builder.RegisterType<OwlFinanceSettingsProvider>().As<IAppSettingsProvider>();
        }
    }
}