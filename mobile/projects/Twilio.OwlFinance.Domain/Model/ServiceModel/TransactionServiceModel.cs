namespace Twilio.OwlFinance.Domain.Model.ServiceModel
{
    public class TransactionServiceModel : BaseServiceModel
    {
		public int TransactionId { get; set; }
        public string DisplayDate { get; set; }
        public string Merchant { get; set; }
        public string Summary { get; set; }
        public string DisplayAmount { get; set; }
		public string MerchantImageUrl { get; set;}
		public string CardNumber { get; set; }
		public string ExpirationDisplayDate { get; set; }
		public string CardHolderName { get; set;}

    }
}
