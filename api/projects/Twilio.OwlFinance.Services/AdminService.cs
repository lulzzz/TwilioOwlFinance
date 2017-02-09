using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Repositories;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api.Admin;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Infrastructure.DataAccess;
using Twilio.OwlFinance.Services.Auth0;
using Twilio.TaskRouter;
using Event = Twilio.OwlFinance.Domain.Model.Data.Event;
using UserAccount = Twilio.OwlFinance.Domain.Model.Data.Account;
using UserAddress = Twilio.OwlFinance.Domain.Model.Data.Address;
using System.Net.Http;
using System.Runtime.Remoting;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.Services
{
    public class AdminService : BaseService, IAdminService, IDisposable
    {
        private readonly IAppSettingsProvider appSettings;
        private readonly IRepository<User> userRepository;
        private readonly OwlFinanceDbContext context;
        private readonly Auth0Service auth0;


        public AdminService(
            IRepository<User> userRepository,
            IAppSettingsProvider appSettings,
            OwlFinanceDbContext context,
            Auth0Service auth0,
            ILogger logger)
            : base(logger)
        {
            this.userRepository = userRepository;
            this.appSettings = appSettings;
            this.context = context;
            this.auth0 = auth0;
        }

        public async Task<ApiResponse<UserModel>> GetUser(int id)
        {
            try
            {
                var user = (await GetUsers()).Data
                    .SingleOrDefault(u => u.Id == id);
                return new ApiResponse<UserModel>(user);

            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<UserModel>>(e);
            }
        }

        public async Task<EnumerableApiResponse<UserModel>> GetUsers()
        {
            try
            {
                var pairs = userRepository.Query()
                    .OfType<Agent>()
                    .Where(a => a.PairedCustomer != null)
                    .Select(a => new {
                        AgentID = a.ID,
                        CustomerID = a.PairedCustomer.ID,
                        AgentName = a.FirstName + " " + a.LastName,
                        CustomerName = a.PairedCustomer.FirstName + " " + a.PairedCustomer.LastName
                    })
                    .ToList();

                var users = userRepository.Query()
                    .Where(u => !u.IsDeleted)
                    .ToList()
                    .Select(u => new UserModel {
                        Id = u.ID,
                        IdentityID = u.IdentityID,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Type = u is Agent ? UserType.Agent : UserType.Customer,
                        LinkedUser = u is Agent
                            ? pairs.Where(p => p.AgentID == u.ID)
                                .Select(p => new SimpleUserModel {
                                    Id = p.CustomerID,
                                    Name = p.CustomerName
                                }).SingleOrDefault()
                            : pairs.Where(p => p.CustomerID == u.ID)
                                .Select(p => new SimpleUserModel {
                                    Id = p.AgentID,
                                    Name = p.AgentName
                                })
                                .SingleOrDefault(),
                        PhoneNumber = u is Agent ? null : ((Customer)u).PhoneNumber,
                        WorkerSid = u is Agent ? ((Agent)u).SID : null
                    });
                return new EnumerableApiResponse<UserModel>(users);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<UserModel>>(e);
            }
        }

        public async Task<EnumerableApiResponse<UserModel>> GetCustomers()
        {
            try
            {
                var users = userRepository.Query()
                    .Where(u => !u.IsDeleted)
                    .OfType<Customer>()
                    .Select(u => new UserModel {
                        Id = u.ID,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Type = UserType.Customer
                    });
                return new EnumerableApiResponse<UserModel>(users);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<UserModel>>(e);
            }
        }

        public async Task<ApiResponse<UserModel>> SaveUser(UserModel model)
        {
            try
            {
                var now = DateTime.UtcNow;
                User user = null;
                if (model.Type == UserType.Agent)
                {
                    var agent = model.Id.HasValue
                        ? userRepository.Query(u => u.ID == model.Id.Value).OfType<Agent>().Include(u => u.PairedCustomer).SingleOrDefault()
                        : new Agent();
                    var linkedUserId = model.LinkedUser?.Id;
                    var customer = userRepository
                        .Query(u => u.ID == linkedUserId)
                        .OfType<Customer>()
                        .SingleOrDefault();

                    agent.SID = model.WorkerSid;
                    agent.PairedCustomer = customer;

                    user = agent;
                }
                else
                {
                    var customer = model.Id.HasValue
                        ? userRepository.Query(u => u.ID == model.Id.Value).OfType<Customer>().SingleOrDefault()
                        : new Customer();

                    customer.PhoneNumber = model.PhoneNumber;
                    customer.ValueLevel = CustomerValueLevel.Gold;
                    customer.Address = CreateDefaultAddress();

                    user = customer;
                }

                user.IdentityID = model.IdentityID;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.ModifiedDate = now;

                if (!model.Id.HasValue)
                {
                    //new user
                    user.CreatedDate = now;
                    userRepository.Add(user);
                }

                await userRepository.SaveChanges();

                if (!model.Id.HasValue && model.Type == UserType.Customer)
                {
                    //new user
                    CreateSeedData(user as Customer);
                }

                return await GetUser(user.ID);
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<UserModel>>(e);
            }
        }

        public async Task<EmptyApiResponse> DeleteUser(int id)
        {
            userRepository.Remove(id);
            await userRepository.SaveChanges();
            return new EmptyApiResponse();
        }


        public async Task<ApiResponse<CreateAgentResponse>> CreateAgentAsync(CreateAgentRequest request)
        {
            try
            {
                var now = DateTime.UtcNow;
                
                if (request == null || !AgentIsValid(request))
                {
                    return new ApiResponse<CreateAgentResponse>(null)
                    {
                        HasError = true,
                        Message = "Insufficient agent information was sent.",
                        StatusCode = StatusCodes.BadRequest
                    };
                }

                if (EmailHasAlias(request))
                {
                    return new ApiResponse<CreateAgentResponse>(null)
                    {
                        HasError = true,
                        Message = "The email address should not contain an agent or customer alias.",
                        StatusCode = StatusCodes.BadRequest
                    };
                }

                var customerEmail = AddEmailAlias(request.Email, "+customer");
                var agentEmail = AddEmailAlias(request.Email, "+agent");

                if (customerEmail == null || agentEmail == null)
                {
                    return new ApiResponse<CreateAgentResponse>(null)
                    {
                        HasError = true,
                        Message = "The email address was invalid.",
                        StatusCode = StatusCodes.BadRequest
                    };
                }

                var taskRouter = new TaskRouterClient(appSettings.Get("twilio:AccountSID"), appSettings.Get("twilio:AuthToken"));

                var worker = taskRouter.AddWorker(
                    appSettings.Get("twilio:WorkspaceSID"),
                    $"Agent {request.FirstName} {request.LastName}",
                    appSettings.Get("twilio:IdleActivitySID"),
                    "{}");

                if (worker.RestException != null)
                {
                    return new ApiResponse<CreateAgentResponse>(null)
                    {
                        HasError = true,
                        Message = worker.RestException.Message,
                        StatusCode = StatusCodes.BadRequest
                    };
                }

                worker.Attributes = JsonConvert.SerializeObject(new
                {
                    skills = new[] { "voice", "sms", "messaging"},
                    sid = worker.Sid
                });

               worker =  taskRouter.UpdateWorker(
                    worker.WorkspaceSid,
                    worker.Sid,
                    worker.ActivitySid,
                    worker.Attributes,
                    worker.FriendlyName);

                if (worker.RestException != null)
                {
                    return new ApiResponse<CreateAgentResponse>(null)
                    {
                        HasError = true,
                        Message = worker.RestException.Message,
                        StatusCode = StatusCodes.BadRequest
                    };
                }

                var auth0Agent = await auth0.CreatePasswordUserAsync(agentEmail, request.Password);
                var auth0Customer = await auth0.CreatePasswordUserAsync(customerEmail, request.Password);

                if (auth0Customer.InvalidRequest || auth0Agent.InvalidRequest)
                {
                    return new ApiResponse<CreateAgentResponse>(null)
                    {
                        HasError = true,
                        Message = "Error creating auth0 users.",
                        StatusCode = StatusCodes.BadRequest
                    };
                }
            
                var customer = new Customer
                {
                    IdentityID = auth0Customer.UserId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Address = CreateDefaultAddress(),
                    PhoneNumber = request.PhoneNumber,
                    ValueLevel = CustomerValueLevel.Gold,
                    CreatedDate = now,
                    ModifiedDate = now,
                    IsActive = true
                };

                var agent = new Agent
                {
                    IdentityID = auth0Agent.UserId,
                    SID = worker.Sid,
                    FirstName = "Agent",
                    LastName = request.LastName,
                    CreatedDate = now,
                    ModifiedDate = now,
                    IsActive = true,
                    PairedCustomer = customer
                };

                CreateSeedData(customer);
                context.Agents.Add(agent);
                await context.SaveChangesAsync();
                
                // Send a hockey app invite
                using (var client = new HttpClient())
                {
                    try
                    {
                        var appId = appSettings.Get("hockeyApp:AppID");
                        var appSecret = appSettings.Get("hockeyApp:Secret");
                        client.BaseAddress = new Uri("https://rink.hockeyapp.net/");
                        client.DefaultRequestHeaders.Add("X-HockeyAppToken", appSecret);
                        var msg = $"Hello {request.FirstName},\n" +
                                  $"An account for Owl Finance has been created for you.\n\n" +
                                  $"The webpage is https://YOUR-WEBPAGE-URL.com\n" +
                                  $"Your username for the mobile app is {customerEmail}. The password is {request.Password}.\n" +
                                  $"Your username for the web site is {agentEmail}. The password is {request.Password}.\n\n" +
                                  $"Please contact YOUR-EMAIL with any questions!\n" +
                                  $"Thanks";
                        var pairs = new List<KeyValuePair<string, string>>
                        {
                             new KeyValuePair<string, string>("first_name", request.FirstName),
                             new KeyValuePair<string, string>("last_name", request.LastName),
                             new KeyValuePair<string, string>("email", request.Email),
                             new KeyValuePair<string, string>("message", msg)
                        };
                        var content = new FormUrlEncodedContent(pairs);
                        var response = await client.PostAsync($"api/2/apps/{appId}/app_users", content);
                        if (!response.IsSuccessStatusCode)
                        {
                            return new ApiResponse<CreateAgentResponse>(null)
                            {
                                HasError = true,
                                Message = "Error creating hockey app invite.",
                                StatusCode = StatusCodes.BadRequest
                            };
                        }
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                }

                var successResponse = new ApiResponse<CreateAgentResponse>(new CreateAgentResponse())
                {
                    HasError = false,
                    Message = "Your account has been created. Please check your email.",
                };

                return successResponse;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<CreateAgentResponse>>(e);
            }
        }


        #region IDisposable Impl
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                    userRepository.Dispose();
                }

                disposed = true;
            }
        }
        #endregion

        private bool AgentIsValid(CreateAgentRequest request)
        {
            if (request == null) return false;

            return !string.IsNullOrWhiteSpace(request.FirstName)
                && !string.IsNullOrWhiteSpace(request.LastName)
                && !string.IsNullOrWhiteSpace(request.Email)
                && !string.IsNullOrWhiteSpace(request.Password)
                && !string.IsNullOrWhiteSpace(request.PhoneNumber);
        }

        private bool EmailHasAlias(CreateAgentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email)) return false;

            var parts = request.Email.Split(new[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) return false;

            var username = parts[0].ToLower();
            return username.EndsWith("+agent") || username.EndsWith("+customer");
        }

        private string AddEmailAlias(string email, string alias)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            var parts = email.Split(new[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) return null;
            return $"{parts[0]}{alias}@{parts[1]}";
        }

        private UserAddress CreateDefaultAddress()
        {
            var now = DateTime.UtcNow;

            return new UserAddress
            {
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
            };
        }

        private void CreateSeedData(Customer customer)
        {
            var now = DateTime.Now;
            var random = new Random();

            //account
            var account = new UserAccount {
                Owner = customer,
                Number = $"1000000001{random.Next(100000, 999999)}",
                AccountType = AccountType.Checking,
                Balance = 193300,
                CreatedDate = now,
                ModifiedDate = now
            };

            context.Accounts.Add(account);
            context.SaveChanges();

            //account debit card
            var card = new PaymentCard {
                Account = account,
                CardNumber = $"1100000001{random.Next(100000, 999999)}",
                ExpirationDate = new DateTime(2020, 12, 31),
                CardType = CardType.Debit,
                CreatedDate = now,
                ModifiedDate = now
            };

            context.PaymentCards.Add(card);
            context.SaveChanges();

            //account transactions
            var ups = context.Merchants.SingleOrDefault(e => e.Name == "UPS");
            var shell = context.Merchants.SingleOrDefault(e => e.Name == "Shell");
            var gap = context.Merchants.SingleOrDefault(e => e.Name == "Gap");
            var nike = context.Merchants.SingleOrDefault(e => e.Name == "Nike");
            var ikea = context.Merchants.SingleOrDefault(e => e.Name == "Ikea");
            var transactions = new[] {
                new Debit {
                    Account = account,
                    Merchant = shell,
                    Amount = -479,
                    Description = "Filled gas",
                    EffectiveDate = now.AddDays(-10),
                    CreatedDate = now,
                    ModifiedDate = now,
                    PaymentCard = card
                },
                new Debit {
                    Account = account,
                    Merchant = ups,
                    Amount = -1295,
                    Description = "Sent mail",
                    EffectiveDate = now.AddDays(-8),
                    CreatedDate = now,
                    ModifiedDate = now,
                    PaymentCard = card
                },
                new Debit {
                    Account = account,
                    Merchant = nike,
                    Amount = -10762,
                    Description = "Unknown",
                    EffectiveDate = now.AddDays(-5),
                    CreatedDate = now,
                    ModifiedDate = now,
                    PaymentCard = card
                },
                new Debit {
                    Account = account,
                    Merchant = gap,
                    Amount = -5634,
                    Description = "Bought jeans",
                    EffectiveDate = now.AddDays(-5),
                    CreatedDate = now,
                    ModifiedDate = now,
                    PaymentCard = card
                }
            };

            context.Transactions.AddRange(transactions);
            context.SaveChanges();

            //account events
            var events = new[] {
                new Event {
                    Account = account,
                    Summary = "Spoke with an Agent",
                    CreatedDate = now.AddDays(-1).Date
                },
                new Event {
                    Account = account,
                    Summary = "Checked balance",
                    CreatedDate = now.AddDays(-2).Date
                },
                new Event {
                    Account = account,
                    Summary = "Transferred money online",
                    CreatedDate = now.AddDays(-3).Date
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();

            //account statements
            var statements = new[] {
                new Statement {
                    Account = account,
                    StartBalance = 354697,
                    EndBalance = 114576,
                    StartDate = now.AddDays(-31),
                    EndDate = now.AddDays(-1),
                    CreatedDate  = now,
                    ModifiedDate = now
                },
                new Statement {
                    Account = account,
                    StartBalance = 89567,
                    EndBalance = 354697,
                    StartDate = now.AddDays(-61),
                    EndDate = now.AddDays(-31),
                    CreatedDate  = now,
                    ModifiedDate = now
                },
                new Statement {
                    Account = account,
                    StartBalance = 298453,
                    EndBalance = 89567,
                    StartDate = now.AddDays(-91),
                    EndDate = now.AddDays(-61),
                    CreatedDate  = now,
                    ModifiedDate = now
                }
            };

            context.Statements.AddRange(statements);
            context.SaveChanges();
        }
    }
}
