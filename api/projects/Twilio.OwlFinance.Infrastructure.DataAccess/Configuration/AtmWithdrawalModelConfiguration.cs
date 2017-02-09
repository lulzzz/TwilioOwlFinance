using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class AtmWithdrawalModelConfiguration : EntityTypeConfiguration<AtmWithdrawal>
    {
        public AtmWithdrawalModelConfiguration()
        {
            Property(e => e.Fee)
                .IsRequired();
        }
    }
}
