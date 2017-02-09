using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class MerchantModelConfiguration : EntityTypeConfiguration<Merchant>
    {
        public MerchantModelConfiguration()
        {
            ToTable("Merchants");

            HasKey(e => e.ID);

            Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);

            Property(e => e.Description)
                .IsOptional()
                .HasMaxLength(1024);

            Property(e => e.ImagePath)
                .IsOptional()
                .HasMaxLength(512)
                .IsUnicode(false);

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
