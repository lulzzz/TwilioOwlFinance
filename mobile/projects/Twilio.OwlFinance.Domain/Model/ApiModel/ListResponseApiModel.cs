namespace Twilio.OwlFinance.Domain.Model.ApiModel
{
	public class ListResponseApiModel
	{
		public ListResponseApiModel()
		{
		}
	}

	public interface IListResponseApiModel<T> : ICanHaveError where T : class
	{
		
	}
}

