namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Number", c => c.String(nullable: false, maxLength: 16, fixedLength: true, unicode: false));
            AddColumn("dbo.Users", "IdentityID", c => c.String(nullable: false, maxLength: 128, unicode: false));
            CreateIndex("dbo.Accounts", "Number", unique: true);
            CreateIndex("dbo.Users", "IdentityID", unique: true);
            DropColumn("dbo.Accounts", "AccountNumber");
            DropColumn("dbo.Transactions", "CheckNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "CheckNumber", c => c.Int());
            AddColumn("dbo.Accounts", "AccountNumber", c => c.String(nullable: false, maxLength: 16, fixedLength: true, unicode: false));
            DropIndex("dbo.Users", new[] { "IdentityID" });
            DropIndex("dbo.Accounts", new[] { "Number" });
            DropColumn("dbo.Users", "IdentityID");
            DropColumn("dbo.Accounts", "Number");
        }
    }
}
