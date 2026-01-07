using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    /// <summary>
    /// Defines a generic repository contract for performing
    /// CRUD and query operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity managed by the repository.
    /// </typeparam>
    public interface IGenericRepository<TEntity>
    {
        #region Read Operations

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>
        /// The entity if found; otherwise, <c>null</c>.
        /// </returns>
        Task<TEntity?> GetAsync(Guid id);

        /// <summary>
        /// Returns the first entity that matches the specified condition,
        /// or <c>null</c> if no entity is found.
        /// </summary>
        /// <param name="predicate">The condition to filter entities.</param>
        /// <returns>
        /// The first matching entity or <c>null</c>.
        /// </returns>
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Retrieves all entities of the given type.
        /// </summary>
        /// <returns>
        /// A list containing all entities.
        /// </returns>
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// Retrieves all entities that match the specified condition.
        /// </summary>
        /// <param name="predicate">The condition to filter entities.</param>
        /// <returns>
        /// A list of matching entities.
        /// </returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Returns the total number of entities.
        /// </summary>
        /// <returns>
        /// The total entity count.
        /// </returns>
        Task<int> CountAsync();

        /// <summary>
        /// Returns the number of entities that match the specified condition.
        /// </summary>
        /// <param name="predicate">The condition to filter entities.</param>
        /// <returns>
        /// The count of matching entities.
        /// </returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Determines whether any entity exists that matches the specified condition.
        /// </summary>
        /// <param name="predicate">The condition to evaluate.</param>
        /// <returns>
        /// <c>true</c> if at least one entity exists; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Query Operations

        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> for all entities.
        /// Enables further filtering, sorting, or projection.
        /// </summary>
        /// <returns>
        /// A queryable collection of entities.
        /// </returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> including the specified related properties.
        /// Useful for eager loading.
        /// </summary>
        /// <param name="propertySelectors">
        /// Expressions specifying related entities to include.
        /// </param>
        /// <returns>
        /// A queryable collection with included navigation properties.
        /// </returns>
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        #endregion

        #region Create Operations

        /// <summary>
        /// Inserts a new entity into the repository.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>
        /// The inserted entity.
        /// </returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts multiple entities into the repository.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        Task InsertRangeAsync(IEnumerable<TEntity> entities);

        #endregion

        #region Update Operations

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>
        /// The updated entity.
        /// </returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates multiple entities in the repository.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        #endregion

        #region Delete Operations

        /// <summary>
        /// Deletes an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes all entities that match the specified condition.
        /// </summary>
        /// <param name="predicate">The condition to identify entities for deletion.</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Permanently deletes an entity by its unique identifier.
        /// This operation bypasses soft-delete mechanisms, if any.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        Task HardDeleteAsync(Guid id);

        #endregion

        #region Save Changes

        /// <summary>
        /// Persists all changes made in the repository to the data store.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the database.
        /// </returns>
        Task<int> SaveChangesAsync();

        #endregion
    }
}
