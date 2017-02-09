using System.Data.Entity.Migrations;
using Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database.SeedData;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    public class OwlFinanceDbMigrationsConfiguration : DbMigrationsConfiguration<OwlFinanceDbContext>
    {
        public OwlFinanceDbMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(OwlFinanceDbContext context)
        {
            base.Seed(context);
#if DEBUG
            DevelopmentData.Seed(context);
#else
                        ProductionData.Seed(context); 
#endif


        }
    }
}
