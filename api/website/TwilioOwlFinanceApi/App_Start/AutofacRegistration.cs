using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Twilio.Infrastructure.Communications;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Repositories;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Interfaces.TaskRouter;
using Twilio.OwlFinance.Infrastructure.DataAccess;
using Twilio.OwlFinance.Infrastructure.DataAccess.Repositories;
using Twilio.OwlFinance.Infrastructure.Logging;
using Twilio.OwlFinance.Infrastructure.Settings;
using Twilio.OwlFinance.Infrastructure.TaskRouter;
using Twilio.OwlFinance.Services;
using Twilio.OwlFinance.Services.Auth0;
using Twilio.TaskRouter;

namespace Twilio.OwlFinance.BankingService
{
    public class AutofacRegistration
    {
        public static void RegisterTypes()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<OwlFinanceSettingsProvider>()
                .As<IAppSettingsProvider>()
                .InstancePerRequest();

            builder.RegisterType<OwlFinanceDbContext>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerRequest();

            builder.RegisterType<TwilioApiSettingsProvider>()
                .As<ITwilioApiSettingsProvider>()
                .InstancePerRequest();

            builder.Register(
                ctx => {
                    var settings = ctx.Resolve<ITwilioApiSettingsProvider>();
                    return new TaskRouterClient(settings.Account.Sid, settings.AuthToken);
                })
                .As<TaskRouterClient>()
                .InstancePerRequest();

            builder.RegisterType<TaskRouterManager>()
                .As<ITaskRouterManager>()
                .InstancePerRequest();

            builder.RegisterType<AppInsightsLogger>()
                .As<ILogger>()
                .InstancePerRequest();

            builder.RegisterType<AdminService>()
                .As<IAdminService>()
                .InstancePerRequest();

            builder.RegisterType<AccountService>()
                .As<IAccountService>()
                .InstancePerRequest();

            builder.RegisterType<CaseService>()
                .As<ICaseService>()
                .InstancePerRequest();

            builder.RegisterType<MerchantService>()
                .As<IMerchantService>()
                .InstancePerRequest();

            builder.RegisterType<PingService>()
                .As<IPingService>()
                .InstancePerRequest();

            builder.RegisterType<SkillService>()
                .As<ISkillService>()
                .InstancePerRequest();

            builder.RegisterType<TransactionService>()
                .As<ITransactionService>()
                .InstancePerRequest();

            builder.RegisterType<AgentService>()
                .As<IAgentService>()
                .InstancePerRequest();

            builder.RegisterType<DocuSignService>()
                .As<IDocuSignService>()
                .InstancePerRequest();

            builder.RegisterType<WeatherService>()
                .As<IWeatherService>()
                .InstancePerRequest();

            builder.RegisterType<SettingsService>()
                .As<ISettingsService>()
                .InstancePerRequest();

            builder.RegisterType<SmsManager>()
                .As<ISmsManager>()
                .InstancePerRequest();

            builder.RegisterType<VoiceCallManager>()
                .As<IVoiceManager>()
                .InstancePerRequest();

            builder.RegisterType<AccountMatcher>()
                .As<AccountMatcher>()
                .InstancePerRequest();

            builder.RegisterType<Auth0Service>()
                .AsSelf()
                .InstancePerRequest();

            var container = builder.Build();

            AppContainer = container;

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public static IContainer AppContainer { get; set; }
    }
}
