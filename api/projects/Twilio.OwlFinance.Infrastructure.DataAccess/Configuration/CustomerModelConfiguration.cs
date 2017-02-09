using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class CustomerModelConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerModelConfiguration()
        {
            ToTable("Customers");

            HasRequired(e => e.Address)
                .WithOptional()
                .Map(cfg => cfg.MapKey("AddressID"))
                .WillCascadeOnDelete(false);

            Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            Property(e => e.ValueLevel)
                .IsRequired();
        }
    }
}
