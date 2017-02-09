namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountsCardsMerchantsTransactions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccountNumber = c.String(nullable: false, maxLength: 16, fixedLength: true, unicode: false),
                        AccountType = c.Byte(nullable: false),
                        Balance = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.PaymentCards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CardNumber = c.String(nullable: false, maxLength: 16, fixedLength: true, unicode: false),
                        ExpirationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CardType = c.Byte(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Accounts", t => t.AccountID)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Amount = c.Long(nullable: false),
                        Description = c.String(maxLength: 1024),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CheckNumber = c.Int(),
                        Fee = c.Long(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        AccountID = c.Int(nullable: false),
                        MerchantID = c.Int(nullable: false),
                        TransferAccountID = c.Int(),
                        PaymentCardID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Accounts", t => t.AccountID)
                .ForeignKey("dbo.Merchants", t => t.MerchantID)
                .ForeignKey("dbo.Accounts", t => t.TransferAccountID)
                .ForeignKey("dbo.PaymentCards", t => t.PaymentCardID)
                .Index(t => t.AccountID)
                .Index(t => t.MerchantID)
                .Index(t => t.TransferAccountID)
                .Index(t => t.PaymentCardID);
            
            CreateTable(
                "dbo.Merchants",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 1024),
                        ImagePath = c.String(maxLength: 512, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "PaymentCardID", "dbo.PaymentCards");
            DropForeignKey("dbo.Transactions", "TransferAccountID", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "MerchantID", "dbo.Merchants");
            DropForeignKey("dbo.Transactions", "AccountID", "dbo.Accounts");
            DropForeignKey("dbo.PaymentCards", "AccountID", "dbo.Accounts");
            DropIndex("dbo.Transactions", new[] { "PaymentCardID" });
            DropIndex("dbo.Transactions", new[] { "TransferAccountID" });
            DropIndex("dbo.Transactions", new[] { "MerchantID" });
            DropIndex("dbo.Transactions", new[] { "AccountID" });
            DropIndex("dbo.PaymentCards", new[] { "AccountID" });
            DropIndex("dbo.Accounts", new[] { "CustomerID" });
            DropTable("dbo.Merchants");
            DropTable("dbo.Transactions");
            DropTable("dbo.PaymentCards");
            DropTable("dbo.Accounts");
        }
    }
}
