using System;
using Foundation;
using Twilio.Video;

namespace OwlFinance.ViewControllers.Delegates
{
	public class VideoRoomDelegate : RoomDelegate
	{
		public Action<string, Room> OnDidConnectToRoom;
		public Action<string> OnDisconnectedWithError;
		public Action<string> OnRoomFailedToConnect;
		public Action<string, Participant> OnParticipantDidConnect;
		public Action<string, Participant> OnParticipantDisconnected;

		[Export("didConnectToRoom:")]
		public override void DidConnectToRoom(Room room)
		{
			var message = $"Connected as {room.LocalParticipant.Identity}.";
			OnDidConnectToRoom?.Invoke(message, room);
		}

		[Export("room:didDisconnectWithError:")]
		public override void DisconnectedWithError(Room room, NSError error)
		{
			var message = $"Disconnected from room with error: {error}";
			OnDisconnectedWithError?.Invoke(message);
		}

		[Export("room:didFailToConnectWithError:")]
		public override void FailedToConnect(Room room, NSError error)
		{
			var message = $"Failed to connect to room with error: {error}";
			OnRoomFailedToConnect?.Invoke(message);
		}

		[Export("room:participantDidConnect:")]
		public override void ParticipantDidConnect(Room room, Participant participant)
		{
			var message = $"{participant.Identity} connected.";
			OnParticipantDidConnect?.Invoke(message, participant);
		}

		[Export("room:participantDidDisconnect:")]
		public override void ParticipantDisconnected(Room room, Participant participant)
		{
			var message = $"{participant.Identity} disconnected.";
			OnParticipantDisconnected?.Invoke(message, participant);
		}
	}
}