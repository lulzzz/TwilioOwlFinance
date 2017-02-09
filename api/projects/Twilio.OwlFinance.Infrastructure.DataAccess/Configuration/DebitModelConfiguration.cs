using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class DebitModelConfiguration : EntityTypeConfiguration<Debit>
    {
        public DebitModelConfiguration()
        {
            HasOptional(e => e.PaymentCard)
                .WithMany(e => e.Debits)
                .Map(cfg => cfg.MapKey("PaymentCardID"))
                .WillCascadeOnDelete(false);
        }
    }
}
