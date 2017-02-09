namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class DocuSignResponseModel
    {
        public string SignUrl { get; set; }
        public string DocumentUrl { get; set; }
        public string CaseID { get; set; }
        public string EnvelopeID { get; set; }
        public string DocumentID { get; set; }
    }
}
