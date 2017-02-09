using System.Data.Entity.ModelConfiguration;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration
{
    public class BalanceTransferModelConfiguration : EntityTypeConfiguration<BalanceTransfer>
    {
        public BalanceTransferModelConfiguration()
        {
            HasRequired(e => e.TransferAccount)
                .WithMany()
                .Map(cfg => cfg.MapKey("TransferAccountID"))
                .WillCascadeOnDelete(false);
        }
    }
}
