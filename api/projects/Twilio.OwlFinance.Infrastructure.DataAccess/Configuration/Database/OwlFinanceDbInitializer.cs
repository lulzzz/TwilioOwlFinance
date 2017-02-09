using System.Data.Entity;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    public class OwlFinanceDbInitializer : MigrateDatabaseToLatestVersion<OwlFinanceDbContext, OwlFinanceDbMigrationsConfiguration>
    {
        protected readonly bool deleteExistingDb;

        public OwlFinanceDbInitializer(bool deleteExistingDb = false)
        {
            this.deleteExistingDb = deleteExistingDb;
        }

        public override void InitializeDatabase(OwlFinanceDbContext context)
        {
            if (this.deleteExistingDb)
            {
                context.Database.Delete();
            }

            base.InitializeDatabase(context);
        }
    }
}
