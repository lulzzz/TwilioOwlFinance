using System;

namespace Twilio.OwlFinance.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string ToLowerCaseString(this Enum value)
        {
            return value.ToString().ToLower();
        }
    }
}
