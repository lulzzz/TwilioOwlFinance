using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class AgentModelConfiguration : EntityTypeConfiguration<Agent>
    {
        public AgentModelConfiguration()
        {
            ToTable("Agents");

            HasOptional(e => e.PairedCustomer)
                .WithOptionalDependent()
                .Map(cfg => cfg.MapKey("PairedCustomerID"))
                .WillCascadeOnDelete(false);

            Property(e => e.SID)
                .IsRequired()
                .HasMaxLength(34)
                .IsUnicode(false)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[] {
                        new IndexAttribute() { IsUnique = true }
                    }));
        }
    }
}
