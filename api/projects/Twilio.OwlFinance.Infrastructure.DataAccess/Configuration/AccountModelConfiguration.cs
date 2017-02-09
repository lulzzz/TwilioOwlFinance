using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class AccountModelConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountModelConfiguration()
        {
            ToTable("Accounts");

            HasKey(e => e.ID);

            HasRequired(e => e.Owner)
                .WithMany(e => e.Accounts)
                .Map(cfg => cfg.MapKey("CustomerID"))
                .WillCascadeOnDelete(false);

            Property(e => e.Number)
                .IsRequired()
                .HasMaxLength(16)
                .IsFixedLength()
                .IsUnicode(false)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[] {
                        new IndexAttribute() { IsUnique = true }
                    }));

            Property(e => e.AccountType)
                .IsRequired();

            Property(e => e.Balance)
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
