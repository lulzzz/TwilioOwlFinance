namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgentCustomerPairs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agents", "PairedCustomerID", c => c.Int());
            CreateIndex("dbo.Agents", "PairedCustomerID");
            AddForeignKey("dbo.Agents", "PairedCustomerID", "dbo.Customers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Agents", "PairedCustomerID", "dbo.Customers");
            DropIndex("dbo.Agents", new[] { "PairedCustomerID" });
            DropColumn("dbo.Agents", "PairedCustomerID");
        }
    }
}
