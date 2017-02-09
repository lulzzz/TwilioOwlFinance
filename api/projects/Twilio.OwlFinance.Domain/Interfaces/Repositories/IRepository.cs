using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model.Data;

namespace Twilio.OwlFinance.Domain.Interfaces.Repositories
{
    public interface IRepository : IDisposable
    {
        Task SaveChanges();
    }

    public interface IRepository<TEntity> : ICanQueryDb<TEntity>, IRepository, IDisposable
        where TEntity : class, IEntity
    {
        TEntity Get(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(int id);
        void Remove(TEntity entity);
    }
}
