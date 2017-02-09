using System;
using System.Collections.Generic;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class Case : IEntity, ICanBeActive, ICanBeDeleted, ICanBeModified
    {
        public Case()
        {
            IsActive = true;
            Events = new HashSet<Event>();
        }

        public int ID { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Account Account { get; set; }

        public virtual Transaction Transaction { get; set; }

        public virtual Agent Agent { get; set; }

        public string Summary { get; set; }

        public string CustomerNotes { get; set; }

        public CaseStatus Status { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
