namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerValueLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "ValueLevel", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "ValueLevel");
        }
    }
}
