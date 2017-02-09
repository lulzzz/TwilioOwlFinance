using System;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class Statement : IEntity, ICanBeDeleted, ICanBeModified
    {
        public Statement()
        { }

        public int ID { get; set; }

        public virtual Account Account { get; set; }

        public long StartBalance { get; set; }

        public long EndBalance { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
