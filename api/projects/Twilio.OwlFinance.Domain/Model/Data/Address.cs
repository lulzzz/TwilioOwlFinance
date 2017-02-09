using System;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public class Address : IEntity, ICanBeActive, ICanBeDeleted, ICanBeModified
    {
        public Address()
        {
            IsActive = true;
        }

        public int ID { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string StateProvince { get; set; }

        public string PostalCode { get; set; }

        public string CountryRegion { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
