using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;


        public Repository(DefaultDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
        } 

        public virtual IQueryable<T> GetAllAsNoTrackingQueryable() =>
            _dbSet.AsNoTracking();

        public virtual IQueryable<T> GetAllQueryable() =>
            _dbSet.AsQueryable();

        public virtual async Task<IEnumerable<T>> GetByCriteria(CancellationToken cancellationToken, 
                                            Expression<Func<T, bool>> filter = null,
                                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync(cancellationToken);
            }
            else
            {
                return await query.ToListAsync(cancellationToken);
            }
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public virtual void AddRange(List<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = _dbSet.Update(entity);
            var propertyValues = await entry.GetDatabaseValuesAsync(cancellationToken);

            if (propertyValues == null)
            {
                throw new Exception("Entity not found");
            }
            else
            {
                entry.OriginalValues.SetValues(propertyValues);
            }

            return entity;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Remove(entity);
        }

        public virtual async Task<T> GetByIdAsync(object id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }
    }
}
