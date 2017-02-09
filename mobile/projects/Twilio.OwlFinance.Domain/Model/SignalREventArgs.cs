using System;

namespace Twilio.OwlFinance.Domain
{
	public class SignalREventArgs : EventArgs
	{
		public bool IsConnected { get; set; }
	}
}