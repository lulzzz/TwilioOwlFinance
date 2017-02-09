using System.Data.Entity;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    public class OwlFinanceDbConfiguration : DbConfiguration
    {
        public OwlFinanceDbConfiguration()
        {
            //SetDatabaseInitializer(new OwlFinanceDbInitializer(true));
        }
    }
}
