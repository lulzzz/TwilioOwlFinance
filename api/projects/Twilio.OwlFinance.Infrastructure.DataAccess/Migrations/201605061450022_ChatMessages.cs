namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatMessages : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ChatMessages", name: "UserID", newName: "RecipientID");
            RenameIndex(table: "dbo.ChatMessages", name: "IX_UserID", newName: "IX_RecipientID");
            AddColumn("dbo.ChatMessages", "SenderID", c => c.Int(nullable: false));
            CreateIndex("dbo.ChatMessages", "SenderID");
            AddForeignKey("dbo.ChatMessages", "SenderID", "dbo.Users", "ID");
            DropColumn("dbo.ChatMessages", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChatMessages", "IsActive", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.ChatMessages", "SenderID", "dbo.Users");
            DropIndex("dbo.ChatMessages", new[] { "SenderID" });
            DropColumn("dbo.ChatMessages", "SenderID");
            RenameIndex(table: "dbo.ChatMessages", name: "IX_RecipientID", newName: "IX_UserID");
            RenameColumn(table: "dbo.ChatMessages", name: "RecipientID", newName: "UserID");
        }
    }
}
