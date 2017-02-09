namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNotesChatMessages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChatMessages", "RecipientID", "dbo.Users");
            DropForeignKey("dbo.ChatMessages", "SenderID", "dbo.Users");
            DropForeignKey("dbo.EventChatMessages", "EventID", "dbo.Events");
            DropForeignKey("dbo.EventChatMessages", "ChatMessageID", "dbo.ChatMessages");
            DropForeignKey("dbo.Notes", "AgentID", "dbo.Agents");
            DropIndex("dbo.ChatMessages", new[] { "RecipientID" });
            DropIndex("dbo.ChatMessages", new[] { "SenderID" });
            DropIndex("dbo.Notes", new[] { "AgentID" });
            DropIndex("dbo.EventChatMessages", new[] { "EventID" });
            DropIndex("dbo.EventChatMessages", new[] { "ChatMessageID" });
            DropTable("dbo.ChatMessages");
            DropTable("dbo.Notes");
            DropTable("dbo.EventChatMessages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EventChatMessages",
                c => new
                    {
                        EventID = c.Int(nullable: false),
                        ChatMessageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventID, t.ChatMessageID });
            
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
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RecipientID = c.Int(nullable: false),
                        SenderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.EventChatMessages", "ChatMessageID");
            CreateIndex("dbo.EventChatMessages", "EventID");
            CreateIndex("dbo.Notes", "AgentID");
            CreateIndex("dbo.ChatMessages", "SenderID");
            CreateIndex("dbo.ChatMessages", "RecipientID");
            AddForeignKey("dbo.Notes", "AgentID", "dbo.Agents", "ID");
            AddForeignKey("dbo.EventChatMessages", "ChatMessageID", "dbo.ChatMessages", "ID", cascadeDelete: true);
            AddForeignKey("dbo.EventChatMessages", "EventID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ChatMessages", "SenderID", "dbo.Users", "ID");
            AddForeignKey("dbo.ChatMessages", "RecipientID", "dbo.Users", "ID");
        }
    }
}
