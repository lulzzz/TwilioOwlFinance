namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventChatMessagesUniqueConstraint : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Customers", new[] { "AddressID" });
            AlterColumn("dbo.Customers", "AddressID", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "AddressID");

            Sql("ALTER TABLE dbo.EventChatMessages ADD CONSTRAINT UQ_ChatMessageID_EventChatMessages UNIQUE (EventID)");
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE dbo.EventChatMessages DROP CONSTRAINT UQ_ChatMessageID_EventChatMessages");

            DropIndex("dbo.Customers", new[] { "AddressID" });
            AlterColumn("dbo.Customers", "AddressID", c => c.Int());
            CreateIndex("dbo.Customers", "AddressID");
        }
    }
}
