using System;
using Twilio.Conversations;

namespace OwlFinance.ViewControllers.Delegates
{
	public class LocalMediaDelegate : TWCLocalMediaDelegate
	{
		public override void DidAddVideoTrack(TWCLocalMedia media, TWCVideoTrack videoTrack)
		{
			Console.WriteLine("DidAddVideoTrack");
		}

		public override void DidFailToAddVideoTrack(TWCLocalMedia media, TWCVideoTrack videoTrack, Foundation.NSError error)
		{
			Console.WriteLine("DidFailToAddVideoTrack");
		}

		public override void DidRemoveVideoTrack(TWCLocalMedia media, TWCVideoTrack videoTrack)
		{
			Console.WriteLine("DidRemoveVideoTrack");
		}
	}
}