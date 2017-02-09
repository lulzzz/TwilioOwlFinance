using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class TransactionModelConfiguration : EntityTypeConfiguration<Transaction>
    {
        public TransactionModelConfiguration()
        {
            ToTable("Transactions");

            HasKey(e => e.ID);

            HasRequired(e => e.Account)
                .WithMany(e => e.Transactions)
                .Map(cfg => cfg.MapKey("AccountID"))
                .WillCascadeOnDelete(false);

            HasRequired(e => e.Merchant)
                .WithMany(e => e.Transactions)
                .Map(cfg => cfg.MapKey("MerchantID"))
                .WillCascadeOnDelete(false);

            Property(e => e.Amount)
                .IsRequired();

            Property(e => e.Description)
                .IsOptional()
                .HasMaxLength(1024);

            Property(e => e.EffectiveDate)
                .IsRequired()
                .HasColumnType("datetime2");

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
