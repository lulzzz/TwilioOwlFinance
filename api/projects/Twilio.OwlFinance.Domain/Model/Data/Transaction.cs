using System;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public abstract class Transaction : IEntity, ICanBeDeleted, ICanBeModified
    {
        public Transaction()
        { }

        public int ID { get; set; }

        public virtual Account Account { get; set; }

        public virtual Merchant Merchant { get; set; }

        public long Amount { get; set; }

        public string Description { get; set; }

        public DateTime EffectiveDate { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
