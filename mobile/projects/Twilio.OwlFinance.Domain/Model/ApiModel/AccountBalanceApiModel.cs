using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Domain.Model.ApiModel
{
	public class AccountBalanceApiModel: ICanHaveError
	{
		public Balance Data { get; set; }
		public string Message { get; set; }
		public bool HasError { get; set; }
		public int StatusCode { get; set; }
	}
}

