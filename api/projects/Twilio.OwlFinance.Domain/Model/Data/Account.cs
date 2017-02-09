using System;
using System.Collections.Generic;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class Account : IEntity, ICanBeActive, ICanBeDeleted, ICanBeModified
    {
        public Account()
        {
            IsActive = true;
            Cards = new HashSet<PaymentCard>();
            Events = new HashSet<Event>();
            Transactions = new HashSet<Transaction>();
            Statements = new HashSet<Statement>();
        }

        public int ID { get; set; }

        public virtual Customer Owner { get; set; }

        public string Number { get; set; }

        public AccountType AccountType { get; set; }

        public long Balance { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<PaymentCard> Cards { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Statement> Statements { get; set; }
    }
}
