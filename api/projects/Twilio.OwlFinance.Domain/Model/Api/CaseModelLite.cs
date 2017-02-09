using System;

namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class CaseModelLite
    {
        public int Id { get; set; }
        public string AccountOwner { get; set; }
        public string AccountOwnerImgUrl { get; set; }
        public string Summary { get; set; }
        public DateTime OpenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
    }
}