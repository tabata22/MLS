using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MLS.Data
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly LoanDbContext DbContext;

        public BaseRepository(LoanDbContext dbContext) => DbContext = dbContext;
        
        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var entity = await DbContext.Set<TEntity>().Where(predicate).FirstOrDefaultAsync(cancellationToken);
            if (entity == null)
                return;
            
            DbContext.Remove(entity);
        }

        public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var entities = await DbContext.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
            DbContext.RemoveRange(entities);
        }

        public virtual async Task<TEntity> GetAsync(object id, CancellationToken cancellationToken = default) 
            => await DbContext.Set<TEntity>().FindAsync(id, cancellationToken);
        
        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) 
            => await DbContext.Set<TEntity>().Where(predicate).FirstOrDefaultAsync(cancellationToken);

        public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) 
            => await DbContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        
        public virtual async Task SaveAsync(TEntity entity, CancellationToken cancellationToken = default) 
            => await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        public virtual async Task SaveManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default) 
            => await DbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        public virtual async Task UpdateOneAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                await Task.FromCanceled(cancellationToken);

            DbContext.Update(entity);

            await Task.CompletedTask;
        }

        public virtual async Task UpdateOneAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity,
            object[] skipProps = null, CancellationToken cancellationToken = default)
        {
            var updateableEntity = await DbContext.Set<TEntity>().FirstAsync(predicate, cancellationToken);

            var entityType = entity.GetType();
            var properties = updateableEntity.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (skipProps != null && skipProps.Contains(property.Name))
                    continue;

                var newValue = entityType.GetProperty(property.Name)?.GetValue(entity);

                property.SetValue(updateableEntity, newValue);
            }

            DbContext.Update(updateableEntity);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (skip != null)
                query = query.Skip(skip.Value);

            if (take != null)
                query = query.Take(take.Value);

            return await query.ToListAsync(cancellationToken);
        }
    }
}