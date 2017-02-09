using System;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public abstract class User : IHaveIdentity, IEntity, ICanBeActive, ICanBeDeleted, ICanBeModified
    {
        public User()
        {
            IsActive = true;
        }

        public int ID { get; set; }

        public string IdentityID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImagePath { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
