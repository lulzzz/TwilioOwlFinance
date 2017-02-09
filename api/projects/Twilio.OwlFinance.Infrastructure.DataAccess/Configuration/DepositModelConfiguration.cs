using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class DepositModelConfiguration : EntityTypeConfiguration<Deposit>
    {
        public DepositModelConfiguration()
        {

        }
    }
}
