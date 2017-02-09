using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class CaseModelConfiguration : EntityTypeConfiguration<Case>
    {
        public CaseModelConfiguration()
        {
            ToTable("Cases");

            HasKey(e => e.ID);
            
            HasRequired(e => e.Customer)
                .WithMany(e => e.Cases)
                .Map(cfg => cfg.MapKey("CustomerID"))
                .WillCascadeOnDelete(false);

            HasOptional(e => e.Account)
                .WithMany()
                .Map(cfg => cfg.MapKey("AccountID"))
                .WillCascadeOnDelete(false);

            HasOptional(e => e.Transaction)
                .WithMany()
                .Map(cfg => cfg.MapKey("TransactionID"))
                .WillCascadeOnDelete(false);

            HasOptional(e => e.Agent)
                .WithMany(e => e.Cases)
                .Map(cfg => cfg.MapKey("AgentID"))
                .WillCascadeOnDelete(false);

            HasMany(e => e.Events)
                .WithMany()
                .Map(cfg => {
                    cfg.MapLeftKey("CaseID");
                    cfg.MapRightKey("EventID");
                    cfg.ToTable("CaseEvents");
                });

            Property(e => e.Summary)
                .IsRequired()
                .HasMaxLength(1024);

            Property(e => e.CustomerNotes)
                .IsOptional()
                .HasMaxLength(1024);

            Property(e => e.Status)
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
