using System;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public interface IEntity
    {
        int ID { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
