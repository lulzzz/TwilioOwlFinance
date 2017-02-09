using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Repositories;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Infrastructure.DataAccess.Extensions;

namespace Twilio.OwlFinance.Infrastructure.DataAccess.Repositories
{
    public class Repository<TEntity> : RepositoryBase, IRepository<TEntity>
        where TEntity : class, IEntity
    {
        public Repository(OwlFinanceDbContext context, ILogger logger)
            : base(context, logger)
        { }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = Context.Set<TEntity>()
                .WithDefaultIncludes()
                //.Where(e => e.IsDeleted == false)
                .Where(predicate ?? (_ => true));
            return query;
        }

        public TEntity Get(int id)
        {
            var entity = this
                .Query(e => e.ID == id)
                .WithDefaultIncludes()
                .SingleOrDefault();
            return entity;
        }

        public void Add(TEntity entity)
        {
            if (entity != null)
            {
                Context.Entry(entity).State = EntityState.Added;
            }
        }

        public void Update(TEntity entity)
        {
            if (entity != null)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Remove(int id)
        {
            Remove(Get(id));
        }

        public void Remove(TEntity entity)
        {
            if (entity != null)
            {
                var entry = Context.Entry(entity);
                if (entity is ICanBeDeleted)
                {
                    // ensure that entity is attached to the DbContext prior to marking it as deleted
                    if (entry.State == EntityState.Detached)
                    {
                        var localEntity = Context.Set<TEntity>().Local
                            .Where(e => e.ID == entity.ID)
                            .SingleOrDefault();
                        if (localEntity != null)
                        {
                            entity = localEntity;
                        }
                        else
                        {
                            Context.Set<TEntity>().Attach(entity);
                        }
                    }

                    ((ICanBeDeleted)entity).IsDeleted = true;
                }
                else
                {
                    entry.State = EntityState.Deleted;
                }
            }
        }
    }
}
