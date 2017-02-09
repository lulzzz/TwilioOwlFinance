namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class Debit : Transaction
    {
        public virtual PaymentCard PaymentCard { get; set; }
    }
}
