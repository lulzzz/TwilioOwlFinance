namespace Twilio.OwlFinance.Domain.Model
{
    public class EmptyApiResponse : ICanHaveError
    {
        public string Message { get; set; }
        public bool HasError { get; set; }
        public int StatusCode { get; set; }
    }
}
