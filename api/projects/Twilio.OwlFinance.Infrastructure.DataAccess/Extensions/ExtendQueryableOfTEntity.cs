using System.Data.Entity;
using System.Linq;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Extensions
{
    public static class ExtendQueryableOfTEntity
    {
        public static IQueryable<TEntity> WithDefaultIncludes<TEntity>(this IQueryable<TEntity> source)
            where TEntity : class, IEntity
        {
            if (source is IQueryable<Account>)
            {
                source = (IQueryable<TEntity>)(source as IQueryable<Account>).WithDefaultIncludes();
            }
            else if (source is IQueryable<Case>)
            {
                source = (IQueryable<TEntity>)(source as IQueryable<Case>).WithDefaultIncludes();
            }
            else if (source is IQueryable<Customer>)
            {
                source = (IQueryable<TEntity>)(source as IQueryable<Customer>).WithDefaultIncludes();
            }
            else if (source is IQueryable<Transaction>)
            {
                source = (IQueryable<TEntity>)(source as IQueryable<Transaction>).WithDefaultIncludes();
            }

            return source;
        }

        public static IQueryable<Account> WithDefaultIncludes(this IQueryable<Account> source)
        {
            return source
                .Include(e => e.Statements)
                .Include(e => e.Transactions);
        }

        public static IQueryable<Case> WithDefaultIncludes(this IQueryable<Case> source)
        {
            return source
                .Include(e => e.Account.Statements)
                .Include(e => e.Agent)
                .Include(e => e.Customer.Address)
                .Include(e => e.Events)
                .Include(e => e.Transaction);
        }

        public static IQueryable<Customer> WithDefaultIncludes(this IQueryable<Customer> source)
        {
            return source
                .Include(e => e.Address);
        }

        public static IQueryable<Transaction> WithDefaultIncludes(this IQueryable<Transaction> source)
        {
            return source
                .Include(e => e.Account.Owner.Address)
                .Include(e => e.Merchant);
        }
    }
}
