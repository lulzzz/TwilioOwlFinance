using System.Data.Entity.Migrations;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    public partial class AccountStatements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Statements",
                c => new {
                    ID = c.Int(nullable: false, identity: true),
                    StartBalance = c.Long(nullable: false),
                    EndBalance = c.Long(nullable: false),
                    StartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    EndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    AccountID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Accounts", t => t.AccountID)
                .Index(t => t.AccountID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Statements", "AccountID", "dbo.Accounts");
            DropIndex("dbo.Statements", new[] { "AccountID" });
            DropTable("dbo.Statements");
        }
    }
}
