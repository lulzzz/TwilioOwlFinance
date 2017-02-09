namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AgentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Agents", t => t.AgentID)
                .Index(t => t.AgentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "AgentID", "dbo.Agents");
            DropIndex("dbo.Notes", new[] { "AgentID" });
            DropTable("dbo.Notes");
        }
    }
}
