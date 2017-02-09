using System;
using OwlFinance.Views.ConversationsControls;
using Twilio.Conversations;

namespace OwlFinance.ViewControllers.Delegates
{
	public class ConversationDelegate : TWCConversationDelegate
	{
		public event Action OnConversationEnded;

		public override void ConversationDidConnectParticipant(TWCConversation conversation, TWCParticipant participant)
		{
			Console.WriteLine($"Participant connected: {participant.Identity}");
		}

		public override void ConversationDidDisconnectParticipant(TWCConversation conversation, TWCParticipant participant)
		{
			Console.WriteLine($"Participant disconnected: {participant.Identity}");
			if (conversation.Participants.Length <= 1) conversation.Disconnect();
		}

		public override void ConversationDidFailToConnectParticipant(TWCConversation conversation, TWCParticipant participant, Foundation.NSError error)
		{
			Console.WriteLine("Participant failed to connect: {0} with error: {1}", participant.Identity, error.Description);
			conversation.Disconnect();
		}

		public override void ConversationEnded(TWCConversation conversation)
		{
			Console.WriteLine("Conversation ended...");
			OnConversationEnded?.Invoke();
			ConversationsControls.Instance.Reset();
		}

		public override void ConversationEnded(TWCConversation conversation, Foundation.NSError error)
		{
			Console.WriteLine($"Conversation ended with error: {error}");
			OnConversationEnded?.Invoke();
		}
	}
}