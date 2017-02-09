namespace Twilio.OwlFinance.Domain.Model
{
    public interface ICanHaveError
    {
        string Message { get; set; }
        bool HasError { get; set; }
        int StatusCode { get; set; }
    }
}