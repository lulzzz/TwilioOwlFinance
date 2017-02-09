using System;
using Foundation;
using Twilio.IPMessaging;

namespace OwlFinance.ViewControllers.Delegates
{
	public class IPMessagingDelegate : TwilioIPMessagingClientDelegate
	{
		public event Action<Message> OnMessageAdded;
		public event EventHandler<EventArgs> OnClientReady;

		[Export("ipMessagingClient:channel:messageAdded:")]
		public void ChannelMessageAdded(TwilioIPMessagingClient client, Channel channel, Message message)
		{
			OnMessageAdded?.Invoke(message);
		}

		[Export("ipMessagingClient:synchronizationStatusChanged:")]
		public override void SyncronizationStatusChanged(TwilioIPMessagingClient client, ClientSynchronizationStatus status)
		{
			if (status == ClientSynchronizationStatus.Completed)
			{
				OnClientReady?.Invoke(this, new EventArgs());
			}
		}

		[Export("ipMessagingClient:errorReceived:")]
		public override void ErrorReceived(TwilioIPMessagingClient client, Error error)
		{
			Console.WriteLine("IPMessagingDelegate.ErrorReceived(): " + error.Description);
		}
	}
}