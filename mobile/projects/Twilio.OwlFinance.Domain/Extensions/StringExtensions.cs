using System;

namespace Twilio.OwlFinance.Domain.Extensions
{
    public static class StringExtensions
    {
        public static bool CompareContains(this string source, string toCheck, StringComparison comparision)
        {
            if (string.IsNullOrEmpty(toCheck) || string.IsNullOrEmpty(source))
                return true;

            return source.IndexOf(toCheck, comparision) >= 0;
        }
    }
}
