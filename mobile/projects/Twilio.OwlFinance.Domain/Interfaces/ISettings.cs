namespace Twilio.OwlFinance.Domain.Interfaces
{
	public interface ISettings
	{
		bool IsAuthenticated { get; }
		string ClientId { get; }
		string AuthToken { get; }
	}
}