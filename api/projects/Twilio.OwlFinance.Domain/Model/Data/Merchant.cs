using System;
using System.Collections.Generic;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class Merchant : IEntity, ICanBeActive, ICanBeDeleted, ICanBeModified
    {
        public Merchant()
        {
            IsActive = true;
            Transactions = new HashSet<Transaction>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
