using System;
using System.Threading.Tasks;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
	public interface ISignalRService
	{
		event MessageReceived OnMessageReceived;
		event EventHandler<SignalREventArgs> OnStateChanged;

		bool IsConnected { get; }
		bool IsConnectedOrConnecting { get; }

		Task ConnectAsync();
		Task SendAsync(string message);
		void Subscribe(string accountId);
	}
}