namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerPhoneNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "PhoneNumber", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "PhoneNumber");
        }
    }
}
