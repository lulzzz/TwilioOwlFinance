using System;
using System.Collections.Generic;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class PaymentCard : IEntity, ICanBeActive, ICanBeDeleted, ICanBeModified
    {
        public PaymentCard()
        {
            IsActive = true;
            Debits = new HashSet<Debit>();
        }

        public int ID { get; set; }

        public virtual Account Account { get; set; }

        public string CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }

        public CardType CardType { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Debit> Debits { get; set; }
    }
}
