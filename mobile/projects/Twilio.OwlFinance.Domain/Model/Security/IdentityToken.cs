using System;

namespace Twilio.OwlFinance.Domain.Model.Security
{
	public class IdentityToken
	{
		public string identityId { get; set; }
		public string name { get; set; }
		public string picture { get; set; }
	}
}