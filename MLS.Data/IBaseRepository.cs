using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MLS.Data
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(object id, CancellationToken cancellationToken = default);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> Exists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task SaveAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task SaveManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

        Task UpdateOneAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task UpdateOneAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity, object[] skipProps = null, CancellationToken cancellationToken = default);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
    }
}