using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class EventModelConfiguration : EntityTypeConfiguration<Event>
    {
        public EventModelConfiguration()
        {
            ToTable("Events");

            HasKey(e => e.ID);

            HasRequired(e => e.Account)
                .WithMany(e => e.Events)
                .Map(cfg => cfg.MapKey("AccountID"))
                .WillCascadeOnDelete(false);

            Property(e => e.Summary)
                .IsRequired()
                .HasMaxLength(64);

            Property(e => e.IsDeleted)
                .IsRequired();

            Property(e => e.CreatedDate)
                .IsRequired()
                .HasColumnType("datetime2");
        }
    }
}
