using System;

namespace Twilio.OwlFinance.Domain.Model.Data
{
	public class Message : BaseDataModel
	{
		public int Id { get; set; }
		public string Summary { get; set; }
		public DateTime LastCorrespondence { get; set; }
	}
}