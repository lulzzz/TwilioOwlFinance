using System;

namespace Twilio.OwlFinance.Domain.Model.Data
{
    public interface ICanBeModified
    {
        DateTime ModifiedDate { get; set; }
    }
}
