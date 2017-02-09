using System.Collections.Generic;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class Customer : User
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            Cases = new HashSet<Case>();
        }

        public Address Address { get; set; }

        public string PhoneNumber { get; set; }

        public CustomerValueLevel ValueLevel { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public virtual ICollection<Case> Cases { get; set; }
    }
}
