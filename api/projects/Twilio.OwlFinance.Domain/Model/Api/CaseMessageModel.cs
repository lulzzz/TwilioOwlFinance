using System;
using System.Collections.Generic;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Domain.Model.Api
{
    public class CaseMessageModel
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public DateTime LastCorrespondence { get; set; }
        public ICollection<Event> CaseEvents { get; set; }
    }
}
