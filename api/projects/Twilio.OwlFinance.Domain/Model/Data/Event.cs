using System;
using System.Collections.Generic;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class Event : IEntity, ICanBeDeleted
    {
        public int ID { get; set; }

        public virtual Account Account { get; set; }

        public string Summary { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
