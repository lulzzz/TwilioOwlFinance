using System;

namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class AccountModel
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal AvailableBalance { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string CustomerValue { get; set; }
        public string LastStatementDate { get; set; }
        public string LastInteractionDate { get; set; }
        public string CustomerSinceDate { get; set; }
    }
}