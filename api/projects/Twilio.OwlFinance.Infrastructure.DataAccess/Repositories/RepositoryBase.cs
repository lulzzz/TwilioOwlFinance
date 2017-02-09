using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Repositories;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Repositories
{
    public abstract class RepositoryBase : IRepository, IDisposable
    {
        private readonly OwlFinanceDbContext context;
        private readonly ILogger logger;

        protected OwlFinanceDbContext Context => context;

        protected ILogger Logger => logger;

        protected RepositoryBase(OwlFinanceDbContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public virtual async Task SaveChanges()
        {
            await this.context.SaveChangesAsync();
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
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }

                this.disposed = true;
            }
        }
        #endregion
    }
}
