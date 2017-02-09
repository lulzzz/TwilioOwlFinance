using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Twilio.OwlFinance.Domain.Model.Data;
using Level = Twilio.OwlFinance.Domain.Model.Data.CustomerValueLevel;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Configuration.Database.SeedData
{
    internal static class ProductionData
    {
        public static void Seed(OwlFinanceDbContext context)
        {
            SeedAddresses(context);
            SeedAgents(context);
            SeedCustomers(context);
            SeedAccounts(context);
            SeedPaymentCards(context);
            SeedMerchants(context);
            SeedTransactions(context);
            SeedCases(context);
            SeedEvents(context);
            SeedCaseEvents(context);
            SeedStatements(context);
        }

        #region Seed Methods
        private static void SeedAddresses(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;

            context.Addresses.AddOrUpdate(
                address => address.PostalCode,
                new[] {
                    new Address {
                        Line1 = "645 Harrison Street",
                        Line2 = "Third Floor",
                        City = "San Francisco",
                        StateProvince = "CA",
                        PostalCode = "94107",
                        CountryRegion = "United States",
                        Latitude = 37.760441m,
                        Longitude = -122.399831m,
                        CreatedDate = now,
                        ModifiedDate = now
                    },
                    new Address {
                        Line1 = "60 Broadway",
                        Line2 = null,
                        City = "San Francisco",
                        StateProvince = "CA",
                        PostalCode = "94111",
                        CountryRegion = "United States",
                        Latitude = 37.799343m,
                        Longitude = -122.398684m,
                        CreatedDate = now,
                        ModifiedDate = now
                    },
                    new Address {
                        Line1 = "3200 E Washington Ave",
                        Line2 = null,
                        City = "Las Vegas",
                        StateProvince = "NV",
                        PostalCode = "89101",
                        CountryRegion = "United States",
                        Latitude = 36.172545m,
                        Longitude = -115.122114m,
                        CreatedDate = now,
                        ModifiedDate = now
                    }
                }
            );

            context.SaveChanges();
        }

        private static void SeedAgents(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;

            context.Agents.AddOrUpdate(
                agent => agent.SID,
                new[] {
                    new Agent {
                        IdentityID = "auth0|IDENTITY-ID-HERE",
                        FirstName = "Jon",
                        LastName = "Davis",
                        SID = "WORKER-SID-HERE",
                        CreatedDate = now,
                        ModifiedDate = now
                    },
                    new Agent {
                        IdentityID = "auth0|IDENTITY-ID-HERE",
                        FirstName = "Sophie",
                        LastName = "Asher",
                        SID = "WORKER-SID-HERE",
                        CreatedDate = now,
                        ModifiedDate = now
                    },
                    new Agent {
                        IdentityID = "auth0|IDENTITY-ID-HERE",
                        FirstName = "Elisa",
                        LastName = "Bellagamba",
                        SID = "WORKER-SID-HERE",
                        CreatedDate = now,
                        ModifiedDate = now
                    }
                }
            );

            context.SaveChanges();
        }

        private static void SeedCustomers(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;
            var address1 = context.Addresses.SingleOrDefault(e => e.PostalCode == "94107");
            var address2 = context.Addresses.SingleOrDefault(e => e.PostalCode == "94111");
            var address3 = context.Addresses.SingleOrDefault(e => e.PostalCode == "89101");

            var customer1 = new Customer {
                IdentityID = "auth0|IDENTITY-ID-HERE",
                FirstName = "Al",
                LastName = "Cook",
                ImagePath = "acook.png",
                Address = address1,
                PhoneNumber = "1111111111",
                ValueLevel = Level.Gold,
                CreatedDate = now.AddDays(-200),
                ModifiedDate = now
            };

            var customer2 = new Customer {
                IdentityID = "auth0|IDENTITY-ID-HERE",
                FirstName = "Peter",
                LastName = "Tan",
                ImagePath = "ptan.png",
                Address = address2,
                PhoneNumber = "1111111111",
                ValueLevel = Level.Gold,
                CreatedDate = now.AddDays(-200),
                ModifiedDate = now
            };

            var customer3 = new Customer {
                IdentityID = "auth0|IDENTITY-ID-HERE",
                FirstName = "Peter",
                LastName = "Lee",
                ImagePath = "plee.png",
                Address = address3,
                PhoneNumber = "1111111111",
                ValueLevel = Level.Silver,
                CreatedDate = now.AddDays(-200),
                ModifiedDate = now
            };

            context.Customers.AddOrUpdate(
                customer => customer.IdentityID,
                new[] { customer1, customer2, customer3 }
            );

            context.SaveChanges();
        }

        private static void SeedAccounts(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;
            var customer1 = context.Customers.SingleOrDefault(c => c.IdentityID == "auth0|IDENTITY-ID-HERE");
            var customer2 = context.Customers.SingleOrDefault(c => c.IdentityID == "auth0|IDENTITY-ID-HERE");
            var customer3 = context.Customers.SingleOrDefault(c => c.IdentityID == "auth0|IDENTITY-ID-HERE");

            context.Accounts.AddOrUpdate(
                account => account.Number,
                new[] {
                    new Account {
                        Owner = customer1,
                        Number = "0504430521209345",
                        AccountType = AccountType.Checking,
                        Balance = 193300,
                        CreatedDate = now.AddDays(-200),
                        ModifiedDate = now
                    },
                    new Account {
                        Owner = customer2,
                        Number = "0104300518205374",
                        AccountType = AccountType.Checking,
                        Balance = 443170,
                        CreatedDate = now.AddDays(-197),
                        ModifiedDate = now
                    },
                    new Account {
                        Owner = customer3,
                        Number = "0404530525201608",
                        AccountType = AccountType.Checking,
                        Balance = -103,
                        CreatedDate = now.AddDays(-189),
                        ModifiedDate = now
                    }
                }
            );

            context.SaveChanges();
        }

        private static void SeedPaymentCards(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;
            var account1 = context.Accounts.SingleOrDefault(e => e.Number == "0504430521209345");
            var account2 = context.Accounts.SingleOrDefault(e => e.Number == "0104300518205374");
            var account3 = context.Accounts.SingleOrDefault(e => e.Number == "0404530525201608");

            context.PaymentCards.AddOrUpdate(
                card => card.CardNumber,
                new[] {
                    new PaymentCard {
                        Account = account1,
                        CardNumber = "1234567891234567",
                        ExpirationDate = new DateTime(2016, 12, 31),
                        CardType = CardType.Debit,
                        CreatedDate = now.AddDays(-100),
                        ModifiedDate = now
                    },
                    new PaymentCard {
                        Account = account2,
                        CardNumber = "5678901234567890",
                        ExpirationDate = new DateTime(2018, 12, 31),
                        CardType = CardType.Debit,
                        CreatedDate = now.AddDays(-100),
                        ModifiedDate = now
                    },
                    new PaymentCard {
                        Account = account3,
                        CardNumber = "9876543210987654",
                        ExpirationDate = new DateTime(2021, 12, 31),
                        CardType = CardType.Debit,
                        CreatedDate = now.AddDays(-100),
                        ModifiedDate = now
                    }
                }
            );

            context.SaveChanges();
        }

        private static void SeedMerchants(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;

            context.Merchants.AddOrUpdate(
                merchant => merchant.Name,
                new[] {
                    new Merchant {
                        Name = "UPS",
                        Description = "UPS is the premier shipping company.",
                        ImagePath = "ups-logo.png",
                        CreatedDate = now.AddDays(-200),
                        ModifiedDate = now
                    },
                    new Merchant {
                        Name = "Shell",
                        Description = "Your number one gas company.",
                        ImagePath = "shell-logo.png",
                        CreatedDate = now.AddDays(-200),
                        ModifiedDate = now
                    },
                    new Merchant {
                        Name = "Gap",
                        Description = "Find something for everyone at Gap.",
                        ImagePath = "gap-logo.png",
                        CreatedDate = now.AddDays(-200),
                        ModifiedDate = now
                    },
                    new Merchant {
                        Name = "Nike",
                        Description = "Get your shoes on at Nike.",
                        ImagePath = "nike-logo.png",
                        CreatedDate = now.AddDays(-200),
                        ModifiedDate = now
                    },
                    new Merchant {
                        Name = "Ikea",
                        Description = "Furniture for your home.",
                        ImagePath = "ikea-logo.png",
                        CreatedDate = now.AddDays(-200),
                        ModifiedDate = now
                    }
                }
            );

            context.SaveChanges();
        }

        private static void SeedTransactions(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;
            var account1 = context.Accounts.SingleOrDefault(e => e.Number == "0504430521209345");
            var account2 = context.Accounts.SingleOrDefault(e => e.Number == "0104300518205374");
            var account3 = context.Accounts.SingleOrDefault(e => e.Number == "0404530525201608");
            var card1 = context.PaymentCards.SingleOrDefault(e => e.CardNumber == "1234567891234567");
            var card2 = context.PaymentCards.SingleOrDefault(e => e.CardNumber == "5678901234567890");
            var card3 = context.PaymentCards.SingleOrDefault(e => e.CardNumber == "9876543210987654");
            var ups = context.Merchants.SingleOrDefault(e => e.Name == "UPS");
            var shell = context.Merchants.SingleOrDefault(e => e.Name == "Shell");
            var gap = context.Merchants.SingleOrDefault(e => e.Name == "Gap");
            var nike = context.Merchants.SingleOrDefault(e => e.Name == "Nike");
            var ikea = context.Merchants.SingleOrDefault(e => e.Name == "Ikea");

            context.Transactions.AddOrUpdate(
                transaction => transaction.Amount,
                new[] {
                    new Debit {
                        Account = account1,
                        Merchant = shell,
                        Amount = -479,
                        Description = "Filled gas",
                        EffectiveDate = now.AddDays(-10),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card1
                    },
                    new Debit {
                        Account = account1,
                        Merchant = ups,
                        Amount = -1295,
                        Description = "Sent mail",
                        EffectiveDate = now.AddDays(-8),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card1
                    },
                    new Debit {
                        Account = account1,
                        Merchant = nike,
                        Amount = -10762,
                        Description = "Unknown",
                        EffectiveDate = now.AddDays(-5),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card1
                    },
                    new Debit {
                        Account = account1,
                        Merchant = gap,
                        Amount = -5634,
                        Description = "Bought jeans",
                        EffectiveDate = now.AddDays(-5),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card1
                    },
                    new Debit {
                        Account = account2,
                        Merchant = ups,
                        Amount = -1347,
                        Description = "Unknown",
                        EffectiveDate = now.AddDays(-5),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card2
                    },
                    new Debit {
                        Account = account2,
                        Merchant = shell,
                        Amount = -1285,
                        Description = "Filled gas",
                        EffectiveDate = now.AddDays(-7),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card2
                    },
                    new Debit {
                        Account = account2,
                        Merchant = ikea,
                        Amount = -1998,
                        Description = "Bought desk",
                        EffectiveDate = now.AddDays(-9),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card2
                    },
                    new Debit {
                        Account = account2,
                        Merchant = nike,
                        Amount = -7499,
                        Description = "Bought shoes",
                        EffectiveDate = now.AddDays(-5),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card2
                    },
                    new Debit {
                        Account = account2,
                        Merchant = shell,
                        Amount = -546,
                        Description = "Filled gas",
                        EffectiveDate = now.AddDays(-18),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card2
                    },
                    new Debit {
                        Account = account3,
                        Merchant = nike,
                        Amount = -7965,
                        Description = "Bought shoes",
                        EffectiveDate = now.AddDays(-5),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card3
                    },
                    new Debit {
                        Account = account3,
                        Merchant = shell,
                        Amount = -126,
                        Description = "Filled gas",
                        EffectiveDate = now.AddDays(-5),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card3
                    },
                    new Debit {
                        Account = account3,
                        Merchant = ikea,
                        Amount = -4175,
                        Description = "Bought desk",
                        EffectiveDate = now.AddDays(-5),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card3
                    },
                    new Debit {
                        Account = account3,
                        Merchant = gap,
                        Amount = -3278,
                        Description = "Bought jeans",
                        EffectiveDate = now.AddDays(-13),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card3
                    },
                    new Debit {
                        Account = account3,
                        Merchant = ups,
                        Amount = -1752,
                        Description = "Sent mail",
                        EffectiveDate = now.AddDays(-8),
                        CreatedDate = now,
                        ModifiedDate = now,
                        PaymentCard = card3
                    }
                }
            );

            context.SaveChanges();
        }

        private static void SeedCases(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;
            var agent1 = context.Agents.SingleOrDefault(e => e.IdentityID == "auth0|IDENTITY-ID-HERE");
            var agent2 = context.Agents.SingleOrDefault(e => e.IdentityID == "auth0|IDENTITY-ID-HERE");
            var agent3 = context.Agents.SingleOrDefault(e => e.IdentityID == "auth0|IDENTITY-ID-HERE");
            var txn1 = context.Transactions.Include(e => e.Account.Owner).SingleOrDefault(e => e.Amount == -479);
            var txn2 = context.Transactions.Include(e => e.Account.Owner).SingleOrDefault(e => e.Amount == -1295);
            var txn3 = context.Transactions.Include(e => e.Account.Owner).SingleOrDefault(e => e.Amount == -5634);
            var txn4 = context.Transactions.Include(e => e.Account.Owner).SingleOrDefault(e => e.Amount == -1347);
            var txn5 = context.Transactions.Include(e => e.Account.Owner).SingleOrDefault(e => e.Amount == -1998);
            var txn6 = context.Transactions.Include(e => e.Account.Owner).SingleOrDefault(e => e.Amount == -3278);
            var txn7 = context.Transactions.Include(e => e.Account.Owner).SingleOrDefault(e => e.Amount == -4175);
            var txn8 = context.Transactions.Include(e => e.Account.Owner).SingleOrDefault(e => e.Amount == -126);

            context.Cases.AddOrUpdate(
                _case => _case.CustomerNotes,
                new[] {
                    new Case {
                        Customer = txn1.Account.Owner,
                        Account = txn1.Account,
                        Transaction = txn1,
                        Agent = agent1,
                        Summary = "Unknown transaction for $4.79",
                        CustomerNotes = "Morbi nec lorem vestibulum, aliquam ipsum a, dictum risus.",
                        Status = CaseStatus.Assigned,
                        CreatedDate = now.AddDays(-2),
                        ModifiedDate = now
                    },
                    new Case {
                        Customer = txn4.Account.Owner,
                        Account = txn4.Account,
                        Transaction = txn4,
                        Agent = agent1,
                        Summary = "Unknown transaction for $13.47",
                        CustomerNotes = "Vestibulum hendrerit, urna vel vestibulum eleifend, tortor felis scelerisque neque, quis cursus enim tortor nec justo.",
                        Status = CaseStatus.Assigned,
                        CreatedDate = now.AddDays(-2),
                        ModifiedDate = now
                    },
                    new Case {
                        Customer = txn6.Account.Owner,
                        Account = txn6.Account,
                        Transaction = txn6,
                        Agent = agent1,
                        Summary = "Unknown transaction for $32.78",
                        CustomerNotes = "Phasellus vitae interdum enim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.",
                        Status = CaseStatus.Closed,
                        CreatedDate = now,
                        ModifiedDate = now
                    },
                    new Case {
                        Customer = txn7.Account.Owner,
                        Account = txn7.Account,
                        Transaction = txn7,
                        Agent = agent1,
                        Summary = "Updating Account Information",
                        CustomerNotes = "Ut ullamcorper ex eu nunc efficitur, vel auctor nisi tincidunt. In at finibus diam, quis ultricies dolor. Mauris consequat justo magna, in ornare eros tristique at. Suspendisse quis faucibus erat.",
                        Status = CaseStatus.Assigned,
                        CreatedDate = now.AddDays(-4),
                        ModifiedDate = now
                    },
                    new Case {
                        Customer = txn5.Account.Owner,
                        Account = txn5.Account,
                        Transaction = txn5,
                        Agent = agent3,
                        Summary = "Unknown transaction for $19.98",
                        CustomerNotes = "Suspendisse potenti. Etiam scelerisque lacus quis ullamcorper fermentum. Mauris auctor faucibus orci nec luctus. Phasellus scelerisque, nunc vitae consectetur suscipit, libero urna pharetra mauris, quis semper diam justo quis nibh.",
                        Status = CaseStatus.Assigned,
                        CreatedDate = now.AddDays(-1),
                        ModifiedDate = now
                    },
                    new Case {
                        Customer = txn8.Account.Owner,
                        Account = txn8.Account,
                        Transaction = txn8,
                        Agent = agent3,
                        Summary = "Lost Debit Card",
                        CustomerNotes = "Integer quis aliquam tortor, pharetra aliquet augue. Fusce nec elit faucibus, laoreet magna sed, ultricies enim. Pellentesque id arcu eu nunc ornare blandit.",
                        Status = CaseStatus.ActionRequired,
                        CreatedDate = now.AddDays(-1),
                        ModifiedDate = now
                    },
                    new Case {
                        Customer = txn2.Account.Owner,
                        Account = txn2.Account,
                        Transaction = txn2,
                        Agent = agent2,
                        Summary = "Updating Account Information",
                        CustomerNotes = "Mauris volutpat ut diam a sodales. Pellentesque nec ex sapien. Fusce ullamcorper felis non arcu consectetur consequat. Etiam felis enim, facilisis sed diam ut, scelerisque commodo nibh.",
                        Status = CaseStatus.Assigned,
                        CreatedDate = now.AddDays(-1),
                        ModifiedDate = now
                    },
                    new Case {
                        Customer = txn3.Account.Owner,
                        Account = txn3.Account,
                        Transaction = txn3,
                        Agent = agent2,
                        Summary = "Unknown transaction for $56.34",
                        CustomerNotes = "Morbi tristique sodales tortor sit amet laoreet. Duis laoreet orci sed lacinia congue. Suspendisse vitae sapien purus. Duis imperdiet lorem vitae arcu ultrices, sed gravida leo ultrices. Sed dignissim, sem rhoncus facilisis luctus, nisl libero aliquet dolor, non ultrices sapien nunc eu urna.",
                        Status = CaseStatus.Assigned,
                        CreatedDate = now.AddDays(-4),
                        ModifiedDate = now
                    }
                }
            );

            context.SaveChanges();
        }

        private static void SeedEvents(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;
            var account1 = context.Accounts.SingleOrDefault(e => e.Number == "0504430521209345");
            var account2 = context.Accounts.SingleOrDefault(e => e.Number == "0104300518205374");
            var account3 = context.Accounts.SingleOrDefault(e => e.Number == "0404530525201608");

            context.Events.AddOrUpdate(
                e => e.CreatedDate,
                new[] {
                    new Event {
                        Account = account1,
                        Summary = "Spoke with an Agent",
                        CreatedDate = now.AddDays(-1).Date
                    },
                    new Event {
                        Account = account1,
                        Summary = "Checked balance",
                        CreatedDate = now.AddDays(-2).Date
                    },
                    new Event {
                        Account = account1,
                        Summary = "Transferred money online",
                        CreatedDate = now.AddDays(-3).Date
                    },
                    new Event {
                        Account = account2,
                        Summary = "Spoke with an Agent",
                        CreatedDate = now.AddDays(-4).Date
                    },
                    new Event {
                        Account = account2,
                        Summary = "Checked balance",
                        CreatedDate = now.AddDays(-5).Date
                    },
                    new Event {
                        Account = account2,
                        Summary = "Transferred money online",
                        CreatedDate = now.AddDays(-6).Date
                    },
                    new Event {
                        Account = account3,
                        Summary = "Spoke with an Agent",
                        CreatedDate = now.AddDays(-7).Date
                    },
                    new Event {
                        Account = account3,
                        Summary = "Checked balance",
                        CreatedDate = now.AddDays(-8).Date
                    },
                    new Event {
                        Account = account3,
                        Summary = "Transferred money online",
                        CreatedDate = now.AddDays(-9).Date
                    }
                }
            );

            context.SaveChanges();
        }

        private static void SeedCaseEvents(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;
            var account1 = context.Accounts.Include(e => e.Events).SingleOrDefault(e => e.Number == "0504430521209345");
            var account2 = context.Accounts.Include(e => e.Events).SingleOrDefault(e => e.Number == "0104300518205374");
            var account3 = context.Accounts.Include(e => e.Events).SingleOrDefault(e => e.Number == "0404530525201608");
            var case1 = context.Cases.Include(e => e.Events).SingleOrDefault(e => e.Summary == "Unknown transaction for $4.79");
            if (!case1.Events.Any())
            {
                foreach (var _event in account1.Events)
                {
                    case1.Events.Add(_event);
                }

                context.SaveChanges();
            }

            var case2 = context.Cases.Include(e => e.Events).SingleOrDefault(e => e.Summary == "Unknown transaction for $13.47");
            if (!case2.Events.Any())
            {
                foreach (var _event in account2.Events)
                {
                    case2.Events.Add(_event);
                }

                context.SaveChanges();
            }

            var case3 = context.Cases.Include(e => e.Events).SingleOrDefault(e => e.Summary == "Unknown transaction for $32.78");
            if (!case3.Events.Any())
            {
                foreach (var _event in account3.Events)
                {
                    case3.Events.Add(_event);
                }

                context.SaveChanges();
            }
        }

        private static void SeedStatements(OwlFinanceDbContext context)
        {
            var now = DateTime.UtcNow;
            var account1 = context.Accounts.SingleOrDefault(e => e.Number == "0504430521209345");
            var account2 = context.Accounts.SingleOrDefault(e => e.Number == "0104300518205374");
            var account3 = context.Accounts.SingleOrDefault(e => e.Number == "0404530525201608");

            context.Statements.AddOrUpdate(
                e => e.StartBalance,
                new[] {
                    new Statement {
                        Account = account1,
                        StartBalance = 354697,
                        EndBalance = 114576,
                        StartDate = now.AddDays(-31),
                        EndDate = now.AddDays(-1),
                        CreatedDate  = now,
                        ModifiedDate = now
                    },
                    new Statement {
                        Account = account1,
                        StartBalance = 89567,
                        EndBalance = 354697,
                        StartDate = now.AddDays(-61),
                        EndDate = now.AddDays(-31),
                        CreatedDate  = now,
                        ModifiedDate = now
                    },
                    new Statement {
                        Account = account1,
                        StartBalance = 298453,
                        EndBalance = 89567,
                        StartDate = now.AddDays(-91),
                        EndDate = now.AddDays(-61),
                        CreatedDate  = now,
                        ModifiedDate = now
                    },
                    new Statement {
                        Account = account2,
                        StartBalance = 210056,
                        EndBalance = 114576,
                        StartDate = now.AddDays(-31),
                        EndDate = now.AddDays(-1),
                        CreatedDate  = now,
                        ModifiedDate = now
                    },
                    new Statement {
                        Account = account2,
                        StartBalance = 195687,
                        EndBalance = 210056,
                        StartDate = now.AddDays(-61),
                        EndDate = now.AddDays(-31),
                        CreatedDate  = now,
                        ModifiedDate = now
                    },
                    new Statement {
                        Account = account2,
                        StartBalance = 123589,
                        EndBalance = 195687,
                        StartDate = now.AddDays(-91),
                        EndDate = now.AddDays(-61),
                        CreatedDate  = now,
                        ModifiedDate = now
                    },
                    new Statement {
                        Account = account3,
                        StartBalance = 210055,
                        EndBalance = 114576,
                        StartDate = now.AddDays(-31),
                        EndDate = now.AddDays(-1),
                        CreatedDate  = now,
                        ModifiedDate = now
                    },
                    new Statement {
                        Account = account3,
                        StartBalance = 176523,
                        EndBalance = 210055,
                        StartDate = now.AddDays(-61),
                        EndDate = now.AddDays(-31),
                        CreatedDate  = now,
                        ModifiedDate = now
                    },
                    new Statement {
                        Account = account3,
                        StartBalance = 92563,
                        EndBalance = 176523,
                        StartDate = now.AddDays(-91),
                        EndDate = now.AddDays(-61),
                        CreatedDate  = now,
                        ModifiedDate = now
                    }
                }
            );

            context.SaveChanges();
        }
        #endregion
    }
}
