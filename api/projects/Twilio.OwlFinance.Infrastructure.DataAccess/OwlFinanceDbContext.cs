using System.Data.Entity;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Infrastructure.DataAccess.Configuration;
using Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database;

namespace Twilio.OwlFinance.Infrastructure.DataAccess
{
    [DbConfigurationType(typeof(OwlFinanceDbConfiguration))]
    public partial class OwlFinanceDbContext : DbContext
    {
        public OwlFinanceDbContext()
            : base("OwlFinanceDbConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<AtmWithdrawal> AtmWithdrawals { get; set; }
        public virtual DbSet<BalanceTransfer> BalanceTransfers { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Debit> Debits { get; set; }
        public virtual DbSet<Deposit> Deposits { get; set; }
        public virtual DbSet<DocuSignLog> DocuSignLogs { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<PaymentCard> PaymentCards { get; set; }
        public virtual DbSet<Statement> Statements { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Configurations.Add(new AccountModelConfiguration());
            builder.Configurations.Add(new AddressModelConfiguration());
            builder.Configurations.Add(new AgentModelConfiguration());
            builder.Configurations.Add(new AtmWithdrawalModelConfiguration());
            builder.Configurations.Add(new BalanceTransferModelConfiguration());
            builder.Configurations.Add(new CaseModelConfiguration());
            builder.Configurations.Add(new CustomerModelConfiguration());
            builder.Configurations.Add(new DebitModelConfiguration());
            builder.Configurations.Add(new DepositModelConfiguration());
            builder.Configurations.Add(new DocuSignLogModelConfiguration());
            builder.Configurations.Add(new EventModelConfiguration());
            builder.Configurations.Add(new MerchantModelConfiguration());
            builder.Configurations.Add(new PaymentCardModelConfiguration());
            builder.Configurations.Add(new StatementModelConfiguration());
            builder.Configurations.Add(new TransactionModelConfiguration());
            builder.Configurations.Add(new UserModelConfiguration());
        }
    }
}
