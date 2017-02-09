namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkillsUniqueIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Skills", "Description", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Skills", new[] { "Description" });
        }
    }
}
