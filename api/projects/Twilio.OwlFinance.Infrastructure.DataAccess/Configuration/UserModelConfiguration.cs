using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class UserModelConfiguration : EntityTypeConfiguration<User>
    {
        public UserModelConfiguration()
        {
            ToTable("Users");

            HasKey(e => e.ID);

            Property(e => e.IdentityID)
                .IsRequired()
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[] {
                        new IndexAttribute() { IsUnique = true }
                    }));

            Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(128);

            Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(128);

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
