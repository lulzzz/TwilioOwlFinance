// http://forums.xamarin.com/discussion/27184/convert-nsdate-to-datetime
using System;
using System.Globalization;
using Foundation;

namespace OwlFinance.Helpers
{
	public static class NSDateExtensions
	{
		static DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public static DateTime ToDateTime(this NSDate date)
		{
			var utcDateTime = reference.AddSeconds(date.SecondsSinceReferenceDate);
			var dateTime = utcDateTime.ToLocalTime();
			return dateTime;
		}

		public static NSDate ToNSDate(this DateTime datetime)
		{
			var utcDateTime = datetime.ToUniversalTime();
			var date = NSDate.FromTimeIntervalSinceReferenceDate((utcDateTime - reference).TotalSeconds);
			return date;
		}

		public static String ToDateTimeString(this NSDate datetime)
		{
			var timeStamp = datetime.ToDateTime();
			var minute = timeStamp.Minute;
			var minuteString = "";
			minuteString = minute < 10 ? ("0" + minute) : minute.ToString();
			var timeDisplay = timeStamp.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(timeStamp.Month) + " " + timeStamp.Hour + ":" + minuteString + " " + timeStamp.ToString("tt");
			return timeDisplay;
		}
	}
}