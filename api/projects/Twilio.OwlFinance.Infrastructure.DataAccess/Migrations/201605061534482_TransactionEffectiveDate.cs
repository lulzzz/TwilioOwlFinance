namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionEffectiveDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "EffectiveDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.Transactions", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Transactions", "EffectiveDate");
        }
    }
}
