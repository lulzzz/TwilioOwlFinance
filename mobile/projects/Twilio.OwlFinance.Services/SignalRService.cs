/*
 * SignalRClient.cs
 * Based off Spencer Schneidenbach's Xamarin.Forms SignalR Sample
 * Github: https://github.com/schneidenbach/
 * Thanks Spencer!
*/

using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Microsoft.AspNet.SignalR.Client;
using Twilio.OwlFinance.Domain;

namespace Twilio.OwlFinance.Services
{
	public class SignalRService : ISignalRService
	{
		private HubConnection connection;
		private IHubProxy eventsHubProxy;
		private string accountId;

		public event MessageReceived OnMessageReceived;
		public event EventHandler<SignalREventArgs> OnStateChanged;

		public ConnectionState ConnectionState { get; private set; } = ConnectionState.Disconnected;

		public bool IsConnected
		{
			get { return ConnectionState == ConnectionState.Connected; }
		}

		public bool IsConnectedOrConnecting
		{
			get { return ConnectionState != ConnectionState.Disconnected; }
		}

		public SignalRService()
		{
			connection = new HubConnection(OwlFinanceUris.SignalRUri);
			connection.TransportConnectTimeout = new TimeSpan(0, 0, 10);
			eventsHubProxy = connection.CreateHubProxy("EventsHub");
			eventsHubProxy?.On<string, string>("MessageReceived", (accountId, message) =>
			{
				OnMessageReceived?.Invoke(accountId, message);
		   	});
			connection.StateChanged += async (obj) => 
			{
				ConnectionState = obj.NewState;
				OnStateChanged?.Invoke(this, new SignalREventArgs { IsConnected = IsConnected });

				if (obj.NewState == ConnectionState.Disconnected)
				{
					await ConnectAsync();
				}
			};
		}

		public async Task SendAsync(string message)
		{
			if (IsConnected)
			{
				await eventsHubProxy?.Invoke("Send", accountId, message);
			}
		}

		public void Subscribe(string accountId)
		{
			this.accountId = accountId;
			eventsHubProxy?.Invoke("Subscribe", accountId);
		}

		public async Task ConnectAsync()
		{
			await Task.Delay(3000);
			await StartAsync();
		}

		private async Task StartAsync()
		{
			if (!IsConnectedOrConnecting)
			{
				try
				{
					await connection.Start();
				}
				catch (Exception)
				{
					// Do nothing
				}
			}
		}

		private void Stop()
		{
			connection.Stop();
		}
	}
}