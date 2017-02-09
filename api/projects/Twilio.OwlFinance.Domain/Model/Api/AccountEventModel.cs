using System;

namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class AccountEventModel
    {
        public DateTime Date { get; set; }
        public string Summary { get; set; }
    }
}
