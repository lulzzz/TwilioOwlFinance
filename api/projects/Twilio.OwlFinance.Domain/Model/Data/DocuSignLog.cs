using System;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class DocuSignLog : IEntity
    {
        public int ID { get; set; }
        public int CaseID { get; set; }
        public string EnvelopeID { get; set; }
        public string DocumentID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
