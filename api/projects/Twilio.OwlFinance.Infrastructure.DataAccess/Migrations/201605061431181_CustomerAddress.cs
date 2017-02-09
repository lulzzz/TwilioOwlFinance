namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Line1 = c.String(nullable: false, maxLength: 64),
                        Line2 = c.String(maxLength: 64),
                        City = c.String(nullable: false, maxLength: 64),
                        StateProvince = c.String(nullable: false, maxLength: 64),
                        PostalCode = c.String(nullable: false, maxLength: 16),
                        CountryRegion = c.String(nullable: false, maxLength: 64),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Customers", "AddressID", c => c.Int());
            CreateIndex("dbo.Customers", "AddressID");
            AddForeignKey("dbo.Customers", "AddressID", "dbo.Addresses", "ID");
            DropColumn("dbo.Customers", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Address", c => c.String(nullable: false, maxLength: 1024));
            DropForeignKey("dbo.Customers", "AddressID", "dbo.Addresses");
            DropIndex("dbo.Customers", new[] { "AddressID" });
            DropColumn("dbo.Customers", "AddressID");
            DropTable("dbo.Addresses");
        }
    }
}
