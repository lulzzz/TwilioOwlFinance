using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Domain.Model.ApiModel
{
    public class CaseCreatedApiModel : ICanHaveError
    {
        public CaseCreatedModel Data { get; set; }
        public string Message { get; set; }
        public bool HasError { get; set; }
        public int StatusCode { get; set; }
    }
}
