using Autofac;
using OwlFinance.Helpers;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Http;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Infrastructure.Http.Clients;
using Twilio.OwlFinance.Infrastructure.Http.Interfaces;
using Twilio.OwlFinance.Infrastructure.Http.Repositories;
using Twilio.OwlFinance.Infrastructure.Logging;
using Twilio.OwlFinance.Services;
using UIKit;

namespace OwlFinance
{
	public class Application
	{
		public static IContainer Container { get; set; }

		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<TransactionService>().As<ITransactionService>();
			builder.RegisterType<AccountService>().As<IAccountService>();
			builder.RegisterType<MessageService>().As<IMessageService>();
			builder.RegisterType<OwlFinanceSettings>().As<ISettings>();
			builder.RegisterType<SignalRService>().As<ISignalRService>().SingleInstance();

			builder.RegisterType<TransactionHttpRepository>().As<ITransactionRepository>();
			builder.RegisterType<MessageHttpRepository>().As<IMessageRepository>();
			builder.RegisterType<AccountHttpRepository>().As<IAccountRepository>();

			builder.RegisterType<ApplicationInsightsLogger>().As<ILogger>();

			builder.RegisterType<OwlFinanceHttpClient>().As<IHttpClient>();

			Container = builder.Build();

			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}