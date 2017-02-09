using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class PaymentCardModelConfiguration : EntityTypeConfiguration<PaymentCard>
    {
        public PaymentCardModelConfiguration()
        {
            ToTable("PaymentCards");

            HasKey(e => e.ID);

            HasRequired(e => e.Account)
                .WithMany(e => e.Cards)
                .Map(cfg => cfg.MapKey("AccountID"))
                .WillCascadeOnDelete(false);

            Property(e => e.CardNumber)
                .IsRequired()
                .HasMaxLength(16)
                .IsFixedLength()
                .IsUnicode(false);

            Property(e => e.ExpirationDate)
                .IsRequired()
                .HasColumnType("datetime2");

            Property(e => e.CardType)
                .IsRequired();

            Property(e => e.IsActive)
                .IsRequired();

            Property(e => e.IsDeleted)
                .IsRequired();

            Property(e => e.CreatedDate)
                .IsRequired()
                .HasColumnType("datetime2");

            Property(e => e.ModifiedDate)
                .IsRequired()
                .HasColumnType("datetime2");
        }
    }
}
