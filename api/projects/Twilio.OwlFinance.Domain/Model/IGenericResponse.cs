namespace Twilio.OwlFinance.Domain.Model
{
    public interface IGenericResponse<T> : ICanHaveError where T : class
    {
        T Data { get; set; }
    }
}