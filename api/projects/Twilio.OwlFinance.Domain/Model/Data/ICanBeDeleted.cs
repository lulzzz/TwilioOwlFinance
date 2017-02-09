namespace Twilio.OwlFinance.Domain.Model.Data
{
    public interface ICanBeDeleted
    {
        bool IsDeleted { get; set; }
    }
}
