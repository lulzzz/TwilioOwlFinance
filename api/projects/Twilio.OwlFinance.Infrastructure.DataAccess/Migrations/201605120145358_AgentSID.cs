namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgentSID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agents", "SID", c => c.String(nullable: false, maxLength: 34, unicode: false));
            CreateIndex("dbo.Agents", "SID", unique: true);
            DropColumn("dbo.Agents", "IsAvailable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Agents", "IsAvailable", c => c.Boolean(nullable: false));
            DropIndex("dbo.Agents", new[] { "SID" });
            DropColumn("dbo.Agents", "SID");
        }
    }
}
