namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Events : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Summary = c.String(nullable: false, maxLength: 64),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Accounts", t => t.AccountID)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.EventChatMessages",
                c => new
                    {
                        EventID = c.Int(nullable: false),
                        ChatMessageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventID, t.ChatMessageID })
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .ForeignKey("dbo.ChatMessages", t => t.ChatMessageID, cascadeDelete: true)
                .Index(t => t.EventID)
                .Index(t => t.ChatMessageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventChatMessages", "ChatMessageID", "dbo.ChatMessages");
            DropForeignKey("dbo.EventChatMessages", "EventID", "dbo.Events");
            DropForeignKey("dbo.Events", "AccountID", "dbo.Accounts");
            DropIndex("dbo.EventChatMessages", new[] { "ChatMessageID" });
            DropIndex("dbo.EventChatMessages", new[] { "EventID" });
            DropIndex("dbo.Events", new[] { "AccountID" });
            DropTable("dbo.EventChatMessages");
            DropTable("dbo.Events");
        }
    }
}
