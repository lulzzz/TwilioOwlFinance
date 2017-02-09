using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Services.Adapters
{
	public class BalanceConverter : IConverter<Balance, string>
	{
		
		public string Convert(Balance input)
		{
			if (input != null)
			{
				return (input.Amount / 100).ToString("C");
			}
			return string.Empty;
		}
	}
}

