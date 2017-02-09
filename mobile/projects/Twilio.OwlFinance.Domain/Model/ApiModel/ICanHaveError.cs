namespace Twilio.OwlFinance.Domain.Model.ApiModel
{
    public interface ICanHaveError
    {
        string Message { get; set; }
        bool HasError { get; set; }
        int StatusCode { get; set; }
    }
}