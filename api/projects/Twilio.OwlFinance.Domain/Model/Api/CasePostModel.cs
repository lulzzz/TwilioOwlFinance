using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Domain.Model.TaskRouter;

namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class CasePostModel
    {
        public int TransactionId { get; set; }
        public CaseType CaseType { get; set; }
        public Skill AgentSkill { get; set; }
    }
}
