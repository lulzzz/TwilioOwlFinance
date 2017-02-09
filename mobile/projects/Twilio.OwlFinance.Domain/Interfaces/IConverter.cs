// ReSharper disable TypeParameterCanBeVariant
namespace Twilio.OwlFinance.Domain.Interfaces
{
    public interface IConverter<TInput, TOutput>
    {
        TOutput Convert(TInput input);
    }
}
