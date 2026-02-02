using System.Linq.Expressions;
namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    /// <summary>
    /// Defines a generic repository contract for managing auditable entities.
    /// This interface provides controlled data access for SaaS applications
    /// without exposing persistence-specific details.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Returns a base queryable for the entity type.
        /// </summary>
        /// <returns>An <see cref="IQueryable{TEntity}"/>.</returns>
        IQueryable<TEntity> Query();

        /// <summary>
        /// Returns a queryable filtered by the given predicate.
        /// </summary>
        /// <param name="predicate">Filter condition.</param>
        /// <returns>An <see cref="IQueryable{TEntity}"/>.</returns>
        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Returns a queryable filtered by a predicate with included navigation properties.
        /// </summary>
        /// <param name="predicate">Filter condition.</param>
        /// <param name="include">Include expression for navigation properties.</param>
        /// <returns>An <see cref="IQueryable{TEntity}"/>.</returns>
        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include);

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>The entity if found; otherwise null.</returns>
        Task<TEntity?> GetByIdAsync(Guid id);

        /// <summary>
        /// Returns the first entity that matches the given predicate.
        /// </summary>
        /// <param name="predicate">Search condition.</param>
        /// <returns>The matching entity or null.</returns>
        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Returns the first entity that matches the predicate with included navigation properties.
        /// </summary>
        /// <param name="predicate">Search condition.</param>
        /// <param name="include">Include expression for navigation properties.</param>
        /// <returns>The matching entity or null.</returns>
        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include);

        /// <summary>
        /// Returns a list of entities that match the given predicate.
        /// </summary>
        /// <param name="predicate">Filter condition.</param>
        /// <returns>A list of matching entities.</returns>
        Task<List<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Determines whether any entity satisfies the given condition.
        /// </summary>
        /// <param name="predicate">Condition to test.</param>
        /// <returns>True if any entity exists; otherwise false.</returns>
        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Returns the count of entities matching the given predicate.
        /// </summary>
        /// <param name="predicate">Filter condition.</param>
        /// <returns>The number of matching entities.</returns>
        Task<int> CountAsync(
            Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Returns a paged subset of entities matching the given predicate.
        /// </summary>
        /// <param name="predicate">Filter condition.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <returns>An <see cref="IQueryable{TEntity}"/> representing the page.</returns>
        IQueryable<TEntity> GetPaged(
            Expression<Func<TEntity, bool>> predicate,
            int page,
            int pageSize);

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <returns>The added entity.</returns>
        Task<TEntity> AddAsync(TEntity entity);
        /// <summary>
        /// Adds multiple entities to the repository.
        /// </summary>
        /// <param name="entities">Entities to add.</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>The updated entity.</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates multiple entities.
        /// </summary>
        /// <param name="entities">Entities to update.</param>
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes an entity using its identifier.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Persists all pending changes to the data store.
        /// </summary>
        /// <returns>The number of affected records.</returns>
        int Complete();

        /// <summary>
        /// Persists all pending changes to the data store asynchronously.
        /// </summary>
        /// <returns>The number of affected records.</returns>
        Task<int> CompleteAsync();
    }
}