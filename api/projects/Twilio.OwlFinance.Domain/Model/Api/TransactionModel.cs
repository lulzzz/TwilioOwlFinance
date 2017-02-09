using System;

namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string CardNumber { get; set; }
        public DateTime CardExpirationDate { get; set; }
        public string MerchantName { get; set; }
        public string MerchantImageUrl { get; set; }
        public string CardHolderName { get; set; }
    }
}