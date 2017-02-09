using System.Collections.Generic;
using System.Linq;
using Twilio.OwlFinance.Domain.Model.TaskRouter;

namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class AgentModel : UserModel
    {
        public AgentModel()
        {
            Skills = Enumerable.Empty<Skill>();
        }

        public string Sid { get; set; }
        public bool IsOffline { get; set; }
        public bool IsAvailable { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
    }
}
