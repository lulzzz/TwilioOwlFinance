using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class AddressModelConfiguration : EntityTypeConfiguration<Address>
    {
        public AddressModelConfiguration()
        {
            ToTable("Addresses");

            HasKey(e => e.ID);

            Property(e => e.Line1)
                .IsRequired()
                .HasMaxLength(64);

            Property(e => e.Line2)
                .IsOptional()
                .HasMaxLength(64);

            Property(e => e.City)
                .IsRequired()
                .HasMaxLength(64);

            Property(e => e.StateProvince)
                .IsRequired()
                .HasMaxLength(64);

            Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(16);

            Property(e => e.CountryRegion)
                .IsRequired()
                .HasMaxLength(64);

            Property(e => e.Latitude)
                .IsRequired()
                .HasPrecision(9, 6);

            Property(e => e.Longitude)
                .IsRequired()
                .HasPrecision(9, 6);

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
