namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkillsAndCases : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cases",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Summary = c.String(nullable: false, maxLength: 1024),
                        Status = c.Byte(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AccountID = c.Int(),
                        AgentID = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        TransactionID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Accounts", t => t.AccountID)
                .ForeignKey("dbo.Agents", t => t.AgentID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .ForeignKey("dbo.Transactions", t => t.TransactionID)
                .Index(t => t.AccountID)
                .Index(t => t.AgentID)
                .Index(t => t.CustomerID)
                .Index(t => t.TransactionID);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 128),
                        Capacity = c.Byte(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Agents", t => t.AgentID)
                .ForeignKey("dbo.Cases", t => t.CaseID)
                .Index(t => t.AgentID)
                .Index(t => t.CaseID);
            
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserID = c.Int(nullable: false),
                        CaseEventID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .ForeignKey("dbo.CaseEvents", t => t.CaseEventID)
                .Index(t => t.UserID)
                .Index(t => t.CaseEventID);
            
            CreateTable(
                "dbo.AgentSkills",
                c => new
                    {
                        AgentID = c.Int(nullable: false),
                        SkillID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AgentID, t.SkillID })
                .ForeignKey("dbo.Agents", t => t.AgentID, cascadeDelete: true)
                .ForeignKey("dbo.Skills", t => t.SkillID, cascadeDelete: true)
                .Index(t => t.AgentID)
                .Index(t => t.SkillID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cases", "TransactionID", "dbo.Transactions");
            DropForeignKey("dbo.ChatMessages", "CaseEventID", "dbo.CaseEvents");
            DropForeignKey("dbo.ChatMessages", "UserID", "dbo.Users");
            DropForeignKey("dbo.CaseEvents", "CaseID", "dbo.Cases");
            DropForeignKey("dbo.CaseEvents", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.Cases", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Cases", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.AgentSkills", "SkillID", "dbo.Skills");
            DropForeignKey("dbo.AgentSkills", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.Cases", "AccountID", "dbo.Accounts");
            DropIndex("dbo.AgentSkills", new[] { "SkillID" });
            DropIndex("dbo.AgentSkills", new[] { "AgentID" });
            DropIndex("dbo.ChatMessages", new[] { "CaseEventID" });
            DropIndex("dbo.ChatMessages", new[] { "UserID" });
            DropIndex("dbo.CaseEvents", new[] { "CaseID" });
            DropIndex("dbo.CaseEvents", new[] { "AgentID" });
            DropIndex("dbo.Cases", new[] { "TransactionID" });
            DropIndex("dbo.Cases", new[] { "CustomerID" });
            DropIndex("dbo.Cases", new[] { "AgentID" });
            DropIndex("dbo.Cases", new[] { "AccountID" });
            DropTable("dbo.AgentSkills");
            DropTable("dbo.ChatMessages");
            DropTable("dbo.CaseEvents");
            DropTable("dbo.Skills");
            DropTable("dbo.Cases");
        }
    }
}
