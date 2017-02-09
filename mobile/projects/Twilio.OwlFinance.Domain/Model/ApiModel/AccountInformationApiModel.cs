using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Domain.Model.ApiModel
{
	public class AccountInformationApiModel : ICanHaveError
	{
		public AccountInformation Data { get; set; }
		public string Message { get; set; }
		public bool HasError { get; set; }
		public int StatusCode { get; set; }
	}
}

