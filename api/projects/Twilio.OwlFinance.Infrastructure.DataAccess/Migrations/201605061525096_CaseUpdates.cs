using System.Data.Entity.Migrations;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    public partial class CaseUpdates : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Cases", new[] { "AgentID" });
            CreateTable(
                "dbo.CaseEvents",
                c => new {
                    CaseID = c.Int(nullable: false),
                    EventID = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.CaseID, t.EventID })
                .ForeignKey("dbo.Cases", t => t.CaseID, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .Index(t => t.CaseID)
                .Index(t => t.EventID);

            AddColumn("dbo.Cases", "CustomerNotes", c => c.String(maxLength: 1024));
            AlterColumn("dbo.Cases", "AgentID", c => c.Int());
            CreateIndex("dbo.Cases", "AgentID");

            Sql("ALTER TABLE dbo.CaseEvents ADD CONSTRAINT UQ_EventID_CaseEvents UNIQUE (EventID)");
        }

        public override void Down()
        {
            Sql("ALTER TABLE dbo.CaseEvents DROP CONSTRAINT UQ_EventID_CaseEvents");

            DropForeignKey("dbo.CaseEvents", "EventID", "dbo.Events");
            DropForeignKey("dbo.CaseEvents", "CaseID", "dbo.Cases");
            DropIndex("dbo.CaseEvents", new[] { "EventID" });
            DropIndex("dbo.CaseEvents", new[] { "CaseID" });
            DropIndex("dbo.Cases", new[] { "AgentID" });
            AlterColumn("dbo.Cases", "AgentID", c => c.Int(nullable: false));
            DropColumn("dbo.Cases", "CustomerNotes");
            DropTable("dbo.CaseEvents");
            CreateIndex("dbo.Cases", "AgentID");
        }
    }
}
