using Twilio.OwlFinance.Domain.Model.ApiModel;

namespace Twilio.OwlFinance.Domain.Model.Data
{
	public class TwilioTokenModel
	{
		public string Identity
		{
			get;
			set;
		}

		public string Token
		{
			get;
			set;
		}
	}

	public class TwilioTokenResponse : ICanHaveError
	{
		public TwilioTokenModel Data { get; set; }
		public bool HasError
		{
			get; set;
		}

		public string Message
		{
			get;set;
		}

		public int StatusCode
		{
			get;set;
		}
	}
}

