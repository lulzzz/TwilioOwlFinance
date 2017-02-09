using System;

namespace Twilio.OwlFinance.Domain.Extensions
{
	public static class ByteExtensions
	{
		public static byte[] GetBytes(this string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}
	}
}

