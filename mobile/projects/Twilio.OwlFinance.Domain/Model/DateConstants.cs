using System;

namespace Twilio.OwlFinance.Domain.Model
{
	public static class DateConstants
	{
		public static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public static TimeSpan TimSpanPadding = new TimeSpan(0, 5, 0);
	}
}

