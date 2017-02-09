namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class AgentTaskModel : AgentModel
    {
        public int CaseId { get; set; }
        public string TaskSid { get; set; }
    }
}
