namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserImagePath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ImagePath", c => c.String(maxLength: 512, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ImagePath");
        }
    }
}
