using System;

namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class CaseModel : CaseModelLite
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountOwnerAddress { get; set; }
        public string AccountOwnerCityState { get; set; }
        public string AccountOwnerPostalCode { get; set; }
        public string AccountOwnerPhone { get; set; }
        public string AccountType { get; set; }
        public decimal AvailableBalance { get; set; }
        public string TxnAmount { get; set; }
        public string TxnDescription { get; set; }
        public DateTime? LastStatementDate { get; set; }
        public string CustomerValue { get; set; }
        public DateTime? LastInteraction { get; set; }
        public DateTime CustomerSince { get; set; }
    }
}