using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class DocuSignLogModelConfiguration : EntityTypeConfiguration<DocuSignLog>
    {
        public DocuSignLogModelConfiguration()
        {
            ToTable("DocuSignLog");

            HasKey(e => e.ID);

            Property(e => e.CaseID).IsRequired();
            Property(e => e.EnvelopeID).IsRequired();
            Property(e => e.DocumentID).IsRequired();

            Property(e => e.CreatedDate)
                .IsRequired()
                .HasColumnType("datetime2");
        }
    }
}
