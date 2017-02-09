using System;
using System.Threading.Tasks;
using Autofac;
using OwlFinance.Managers.EventActions;
using Twilio.OwlFinance.Domain.Interfaces.Services;

namespace OwlFinance.Managers
{
	public static class SignalRManager
	{
		private static ISignalRService SignalRService => Application.Container.Resolve<ISignalRService>();
		
		private static void HandleMessageReceieved(string accountId, string message)
		{
			BaseEventAction action;
			if (message.ToLower().StartsWith("docusign_"))
			{
				var docusignUrl = message.Split('_')[1];
				action = new DocuSignEvent();
				action.Fire(docusignUrl);
			}
			else if (message.ToLower() == "closecase")
			{
				action = new CloseCaseEvent();
				action.Fire();
			}
			else if (message.ToLower() == "incomingcall")
			{
				action = new RespondingToCallEvent();
				action.Fire();
			}
		}

		public static void RegisterEventHandlers()
		{
			SignalRService.OnMessageReceived += HandleMessageReceieved;
			SignalRService.OnStateChanged += (sender, e) =>
			{
				if (e.IsConnected)
				{
					Console.WriteLine("Connected to SignalR server...");
					// Subscribe to SignalR Events
					Subscribe(AppSettingsManager.AccountId);
					Console.WriteLine("Subscribed to group...");
				}
				else
				{
					Console.WriteLine("Not connecting to SignalR server...");
				}
			};
		}

		public static void Subscribe(string accountId)
		{
			SignalRService.Subscribe(accountId);
		}

		public static async Task SendAsync(string message)
		{
			await SignalRService.SendAsync(message);
		}
	}
}