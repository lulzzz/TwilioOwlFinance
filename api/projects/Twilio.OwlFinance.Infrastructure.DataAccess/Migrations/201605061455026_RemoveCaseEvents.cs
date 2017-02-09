namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCaseEvents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CaseEvents", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.CaseEvents", "CaseID", "dbo.Cases");
            DropForeignKey("dbo.ChatMessages", "CaseEventID", "dbo.CaseEvents");
            DropIndex("dbo.CaseEvents", new[] { "AgentID" });
            DropIndex("dbo.CaseEvents", new[] { "CaseID" });
            DropIndex("dbo.ChatMessages", new[] { "CaseEventID" });
            DropColumn("dbo.ChatMessages", "CaseEventID");
            DropTable("dbo.CaseEvents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CaseEvents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Notes = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AgentID = c.Int(nullable: false),
                        CaseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ChatMessages", "CaseEventID", c => c.Int(nullable: false));
            CreateIndex("dbo.ChatMessages", "CaseEventID");
            CreateIndex("dbo.CaseEvents", "CaseID");
            CreateIndex("dbo.CaseEvents", "AgentID");
            AddForeignKey("dbo.ChatMessages", "CaseEventID", "dbo.CaseEvents", "ID");
            AddForeignKey("dbo.CaseEvents", "CaseID", "dbo.Cases", "ID");
            AddForeignKey("dbo.CaseEvents", "AgentID", "dbo.Agents", "ID");
        }
    }
}
