namespace Twilio.OwlFinance.Domain.Model.Security
{
	public class DecodedToken
	{
		public string Iss;
		public string Sub;
		public string Aud;
		public long Exp;
		public long Iat;
	}
}

