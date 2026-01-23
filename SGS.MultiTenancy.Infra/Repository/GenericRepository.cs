using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Infra.DataContext;
using System.Linq.Expressions;
using System.Reflection;

namespace SGS.MultiTenancy.Infra.Repository
{
    /// <summary>
    /// Provides a generic repository implementation for auditable entities.
    /// This implementation encapsulates data access logic while enforcing
    /// auditing and soft-delete rules for a SaaS environment.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The database context.
        /// </summary>
        protected readonly AppDbContext _context;

        /// <summary>
        /// The DbSet corresponding to the entity type.
        /// </summary>
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Provides information about the current authenticated user.
        /// </summary>
        protected readonly ICurrentUser? _currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="currentUser">The current user context.</param>
        public GenericRepository(
            AppDbContext context,
            ICurrentUser? currentUser)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _currentUser = currentUser;
        }

        /// <summary>
        /// Returns a base queryable for the entity type.
        /// </summary>
        public IQueryable<TEntity> Query()
            => _dbSet.AsQueryable();

        /// <summary>
        /// Returns a queryable filtered by the specified predicate.
        /// </summary>
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
            => _dbSet.Where(predicate);

        /// <summary>
        /// Returns a queryable filtered by the specified predicate with included navigation properties.
        /// </summary>
        public IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            return include(_dbSet.Where(predicate));
        }

        /// <summary>
        /// Retrieves an entity by its identifier.
        /// </summary>
        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            var idProperty =
                typeof(TEntity).GetProperty("Id") ??
                typeof(TEntity).GetProperty("ID");

            if (idProperty == null)
            {
                throw new InvalidOperationException(
                    $"Entity {typeof(TEntity).Name} must define an Id or ID property of type Guid.");
            }

            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(parameter, idProperty);
            var equals = Expression.Equal(property, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equals, parameter);

            return await _dbSet.FirstOrDefaultAsync(lambda);
        }

        /// <summary>
        /// Returns the first entity that matches the specified predicate.
        /// </summary>
        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Returns the first entity that matches the predicate with included navigation properties.
        /// </summary>
        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            return await include(_dbSet.Where(predicate)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns a list of entities matching the specified predicate.
        /// </summary>
        public async Task<List<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Determines whether any entity satisfies the given predicate.
        /// </summary>
        public async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// Returns the count of entities matching the specified predicate.
        /// </summary>
        public async Task<int> CountAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Returns a paged subset of entities matching the specified predicate.
        /// </summary>
        public IQueryable<TEntity> GetPaged(
            Expression<Func<TEntity, bool>> predicate,
            int page,
            int pageSize)
        {
            return _dbSet
                .Where(predicate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        /// <summary>
        /// Adds a new entity and applies audit information.
        /// </summary>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            PropertyInfo? createdOnProp = typeof(TEntity).GetProperty("CreateOn");
            PropertyInfo? createdByProp = typeof(TEntity).GetProperty("CreateBy");

            if (createdOnProp != null && createdOnProp.CanWrite)
            {
                createdOnProp.SetValue(entity, DateTime.UtcNow);
            }

            if (createdByProp != null && createdByProp.CanWrite)
            {
                createdByProp.SetValue(entity, _currentUser?.UserId ?? new Guid());
            }

            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Adds multiple entities and applies audit information.
        /// </summary>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            PropertyInfo? createdOnProp = typeof(TEntity).GetProperty("CreateOn");
            PropertyInfo? createdByProp = typeof(TEntity).GetProperty("CreateBy");

            foreach (var entity in entities)
            {
                if (createdOnProp?.CanWrite == true)
                {
                    createdOnProp.SetValue(entity, DateTime.UtcNow);
                }

                if (createdByProp?.CanWrite == true)
                {
                    createdByProp.SetValue(entity, _currentUser?.UserId ?? Guid.Empty);
                }
            }

            await _dbSet.AddRangeAsync(entities);

        }

        /// <summary>
        /// Updates an entity and applies audit information.
        /// </summary>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            PropertyInfo? lastUpdateOnProp = typeof(TEntity).GetProperty("LastUpdateOn");
            PropertyInfo? lastUpdateByProp = typeof(TEntity).GetProperty("LastUpdateBy");

            if (lastUpdateOnProp?.CanWrite == true)
            {
                lastUpdateOnProp.SetValue(entity, DateTime.UtcNow);
            }

            if (lastUpdateByProp?.CanWrite == true)
            {
                lastUpdateByProp.SetValue(entity, _currentUser?.UserId ?? Guid.Empty);
            }

            _dbSet.Update(entity);
            return await Task.FromResult(entity);
        }


        /// <summary>
        /// Updates multiple entities and applies audit information.
        /// </summary>
        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            PropertyInfo? lastUpdateOnProp = typeof(TEntity).GetProperty("LastUpdateOn");
            PropertyInfo? lastUpdateByProp = typeof(TEntity).GetProperty("LastUpdateBy");

            foreach (var entity in entities)
            {
                if (lastUpdateOnProp?.CanWrite == true)
                {
                    lastUpdateOnProp.SetValue(entity, DateTime.UtcNow);
                }

                if (lastUpdateByProp?.CanWrite == true)
                {
                    lastUpdateByProp.SetValue(entity, _currentUser?.UserId ?? Guid.Empty);
                }
            }

            _dbSet.UpdateRange(entities);
            await Task.CompletedTask;
        }


        /// <summary>
        /// Soft-deletes an entity by its identifier.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        /// <summary>
        /// Soft-deletes the specified entity and applies audit information.
        /// </summary>
        public async Task DeleteAsync(TEntity entity)
        {
            PropertyInfo? lastUpdateOnProp = typeof(TEntity).GetProperty("LastUpdateOn");
            PropertyInfo? lastUpdateByProp = typeof(TEntity).GetProperty("LastUpdateBy");

            if (lastUpdateOnProp?.CanWrite == true)
            {
                lastUpdateOnProp.SetValue(entity, DateTime.UtcNow);
            }

            if (lastUpdateByProp?.CanWrite == true)
            {
                lastUpdateByProp.SetValue(entity, _currentUser?.UserId ?? Guid.Empty);
            }
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }
        /// <summary>
        /// Persists all pending changes to the database.
        /// </summary>
        public int Complete()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Persists all pending changes to the database asynchronously.
        /// </summary>
        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
