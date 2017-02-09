namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSkills : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AgentSkills", "AgentID", "dbo.Agents");
            DropForeignKey("dbo.AgentSkills", "SkillID", "dbo.Skills");
            DropIndex("dbo.Skills", new[] { "Description" });
            DropIndex("dbo.AgentSkills", new[] { "AgentID" });
            DropIndex("dbo.AgentSkills", new[] { "SkillID" });
            DropTable("dbo.Skills");
            DropTable("dbo.AgentSkills");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AgentSkills",
                c => new
                    {
                        AgentID = c.Int(nullable: false),
                        SkillID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AgentID, t.SkillID });
            
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
            
            CreateIndex("dbo.AgentSkills", "SkillID");
            CreateIndex("dbo.AgentSkills", "AgentID");
            CreateIndex("dbo.Skills", "Description", unique: true);
            AddForeignKey("dbo.AgentSkills", "SkillID", "dbo.Skills", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AgentSkills", "AgentID", "dbo.Agents", "ID", cascadeDelete: true);
        }
    }
}
