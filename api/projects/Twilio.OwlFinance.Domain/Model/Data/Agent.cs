using System.Collections.Generic;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class Agent : User
    {
        public Agent()
        {
            Cases = new HashSet<Case>();
        }

        public string SID { get; set; }

        public virtual ICollection<Case> Cases { get; set; }

        public Customer PairedCustomer { get; set; }
    }
}
