using System;
using System.Linq;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Interfaces.Repositories;
using Twilio.OwlFinance.Domain.Model.Data;
using System.Data.Entity;

namespace Twilio.OwlFinance.Services
{
    public class AccountMatcher : IDisposable
    {
        private readonly IRepository<User> userRepository;

        public AccountMatcher(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Customer> FindPairedCustomerForAgentAsync(string identityID)
        {
            if (string.IsNullOrWhiteSpace(identityID))
            {
                throw new ArgumentException("Agent email cannot be empty");
            }

            var customer = await userRepository.Query()
                .OfType<Agent>()
                .Where(u => u.IdentityID == identityID)
                .Select(u => u.PairedCustomer)
                .SingleOrDefaultAsync();
            
            if (customer == null)
            {
                throw new ArgumentException("No paired account found.");
            }

            return customer;
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
                    userRepository.Dispose();
                }

                disposed = true;
            }
        }
        #endregion
    }
}
