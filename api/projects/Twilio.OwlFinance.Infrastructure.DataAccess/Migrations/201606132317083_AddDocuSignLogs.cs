namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocuSignLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocuSignLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CaseID = c.Int(nullable: false),
                        EnvelopeID = c.String(nullable: false),
                        DocumentID = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DocuSignLog");
        }
    }
}
