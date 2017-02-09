using System;
using System.Linq;
using System.Linq.Expressions;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Domain.Interfaces.Repositories
{
    public interface ICanQueryDb<TEntity>
        where TEntity : class, IEntity
    {
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null);
    }
}
