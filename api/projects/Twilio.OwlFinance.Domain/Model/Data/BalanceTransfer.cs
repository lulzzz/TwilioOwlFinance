namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class BalanceTransfer : Transaction
    {
        public virtual Account TransferAccount { get; set; }
    }
}
