using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class StatementModelConfiguration : EntityTypeConfiguration<Statement>
    {
        public StatementModelConfiguration()
        {
            ToTable("Statements");

            HasKey(e => e.ID);

            HasRequired(e => e.Account)
                .WithMany(e => e.Statements)
                .Map(cfg => cfg.MapKey("AccountID"))
                .WillCascadeOnDelete(false);

            Property(e => e.StartBalance)
                .IsRequired();

            Property(e => e.EndBalance)
                .IsRequired();

            Property(e => e.StartDate)
                .IsRequired()
                .HasColumnType("datetime2");

            Property(e => e.EndDate)
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
