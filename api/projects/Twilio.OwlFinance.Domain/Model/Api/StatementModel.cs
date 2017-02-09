using System;

namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class StatementModel
    {
        public DateTime Date { get; set; }
        public string Month { get; set; }
        public decimal StartBalance { get; set; }
        public decimal EndBalance { get; set; }
    }
}
