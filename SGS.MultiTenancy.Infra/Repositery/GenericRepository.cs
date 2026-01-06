using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Infra.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Infra.Repositery
{
    /// <summary>
    /// Generic repository implementation for Entity Framework Core.
    /// Provides CRUD operations and query capabilities for entity types.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context, must inherit from DbContext.</typeparam>
    /// <typeparam name="TEntity">The type of entity this repository manages.</typeparam>
    /// <remarks>
    /// This repository implements the Repository Pattern to abstract data access logic.
    /// It provides automatic audit trail support and soft delete functionality.
    /// </remarks>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// The database context instance used by this repository.
        /// </summary>
        protected readonly AppDbContext _dbContext;

        /// <summary>
        /// The DbSet for the entity type managed by this repository.
        /// </summary>
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <param name="dbContext">The database context to be used by this repository.</param>
        /// <exception cref="ArgumentNullException">Thrown when dbContext is null.</exception>
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        #region Read Operations

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve (Guid type).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the entity if found; otherwise, null.
        /// </returns>
        /// <remarks>
        /// Uses Entity Framework's FindAsync method which may retrieve entities from cache.
        /// </remarks>
        public virtual async Task<TEntity?> GetAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Returns the first entity that satisfies the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the first matching entity if found; otherwise, null.
        /// </returns>
        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Retrieves all entities of type TEntity.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of all entities.
        /// </returns>
        /// <remarks>
        /// This method loads all entities into memory. Consider using GetAll() with filters for large datasets.
        /// </remarks>
        public virtual async Task<List<TEntity>> GetAllListAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Retrieves all entities that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of matching entities.
        /// </returns>
        public virtual async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Returns the total number of entities in the repository.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the total count of entities.
        /// </returns>
        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        /// <summary>
        /// Returns the number of entities that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the count of matching entities.
        /// </returns>
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Determines whether any entity satisfies the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains true if any entity satisfies the condition; otherwise, false.
        /// </returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        #endregion

        #region Query Operations

        /// <summary>
        /// Returns an IQueryable for the entity type, allowing for additional query composition.
        /// </summary>
        /// <returns>An IQueryable&lt;TEntity&gt; that can be used for additional query composition.</returns>
        /// <remarks>
        /// This method returns a queryable that hasn't been executed yet. 
        /// Query execution is deferred until the results are enumerated.
        /// </remarks>
        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        /// <summary>
        /// Returns an IQueryable that includes specified related entities (eager loading).
        /// </summary>
        /// <param name="propertySelectors">Lambda expressions specifying navigation properties to include.</param>
        /// <returns>An IQueryable&lt;TEntity&gt; with specified navigation properties included.</returns>
        /// <remarks>
        /// Use this method to avoid N+1 query problems by loading related data in a single query.
        /// </remarks>
        public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = _dbSet.AsQueryable();

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        #endregion

        #region Create Operations

        /// <summary>
        /// Inserts a new entity into the repository.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the inserted entity with any database-generated values.
        /// </returns>
        /// <remarks>
        /// Automatically sets audit fields (CreationTime and CreatorUserId) if they exist on the entity.
        /// </remarks>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var entry = await _dbSet.AddAsync(entity);

            // Set audit fields if they exist
            SetAuditFields(entity);

            return entry.Entity;
        }

        /// <summary>
        /// Inserts multiple entities into the repository in a single operation.
        /// </summary>
        /// <param name="entities">The collection of entities to insert.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// More efficient than multiple individual inserts for bulk operations.
        /// Automatically sets audit fields for each entity.
        /// </remarks>
        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetAuditFields(entity);
            }

            await _dbSet.AddRangeAsync(entities);
        }

        #endregion

        #region Update Operations

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the updated entity.
        /// </returns>
        /// <remarks>
        /// Automatically sets modification audit fields (LastModificationTime) if they exist on the entity.
        /// If the entity is not being tracked, it will be attached to the context.
        /// </remarks>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            // Set modification audit fields if they exist
            SetModificationAuditFields(entity);

            var entry = _dbSet.Update(entity);
            return await Task.FromResult(entry.Entity);
        }

        /// <summary>
        /// Updates multiple entities in the repository in a single operation.
        /// </summary>
        /// <param name="entities">The collection of entities to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// More efficient than multiple individual updates for bulk operations.
        /// Automatically sets modification audit fields for each entity.
        /// </remarks>
        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetModificationAuditFields(entity);
            }

            _dbSet.UpdateRange(entities);
            await Task.CompletedTask;
        }

        #endregion

        #region Delete Operations

        /// <summary>
        /// Deletes an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete (Guid type).</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Performs a soft delete if the entity has an IsDeleted property; otherwise performs a hard delete.
        /// If the entity is not found, no action is taken.
        /// </remarks>
        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// If the entity has an IsDeleted property, performs a soft delete by setting IsDeleted to true.
        /// Otherwise, performs a hard delete by removing the entity from the database.
        /// </remarks>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            // Try soft delete first (if entity has IsDeleted property)
            if (TrySoftDelete(entity))
            {
                await UpdateAsync(entity);
            }
            else
            {
                // Hard delete
                _dbSet.Remove(entity);
            }
        }

        /// <summary>
        /// Deletes all entities that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Uses soft delete if entities have IsDeleted property; otherwise uses hard delete.
        /// This method loads all matching entities into memory before deleting them.
        /// For large datasets, consider using a different approach to avoid memory issues.
        /// </remarks>
        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await GetAllListAsync(predicate);

            foreach (var entity in entities)
            {
                await DeleteAsync(entity);
            }
        }

        /// <summary>
        /// Permanently deletes an entity by its unique identifier (bypasses soft delete).
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete (Guid type).</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method completely removes the entity from the database.
        /// Use with caution as this action cannot be undone through soft delete recovery.
        /// </remarks>
        public virtual async Task HardDeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        #endregion

        #region Save Operations

        /// <summary>
        /// Saves all changes made in this repository to the underlying database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// This method should be called after performing insert, update, or delete operations
        /// to persist changes to the database. Multiple operations can be batched before calling this method.
        /// </remarks>
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Sets audit trail fields on an entity before insertion.
        /// </summary>
        /// <param name="entity">The entity on which to set audit fields.</param>
        /// <remarks>
        /// Sets CreationTime to current UTC time and CreatorUserId if the properties exist.
        /// CreatorUserId requires implementation of current user retrieval from authentication system.
        /// This method uses reflection to check for and set properties dynamically.
        /// </remarks>
        private void SetAuditFields(TEntity entity)
        {
            var now = DateTime.UtcNow;

            // Set CreationTime if property exists
            var creationTimeProp = entity.GetType().GetProperty("CreationTime");
            if (creationTimeProp != null && creationTimeProp.CanWrite)
            {
                creationTimeProp.SetValue(entity, now);
            }

            // Set CreatorUserId if property exists and current user is available
            var creatorUserIdProp = entity.GetType().GetProperty("CreatorUserId");
            if (creatorUserIdProp != null && creatorUserIdProp.CanWrite)
            {
                // TODO: Implement current user ID retrieval from authentication system
                // creatorUserIdProp.SetValue(entity, currentUserId);
            }

            // Note: For multi-tenant applications, consider setting TenantId here if needed
        }

        /// <summary>
        /// Sets modification audit fields on an entity before update.
        /// </summary>
        /// <param name="entity">The entity on which to set modification audit fields.</param>
        /// <remarks>
        /// Sets LastModificationTime to current UTC time if the property exists.
        /// This method uses reflection to check for and set properties dynamically.
        /// </remarks>
        private void SetModificationAuditFields(TEntity entity)
        {
            var now = DateTime.UtcNow;

            // Set LastModificationTime if property exists
            var lastModTimeProp = entity.GetType().GetProperty("LastModificationTime");
            if (lastModTimeProp != null && lastModTimeProp.CanWrite)
            {
                lastModTimeProp.SetValue(entity, now);
            }

            // Set LastModifierUserId if property exists and current user is available
            var lastModifierUserIdProp = entity.GetType().GetProperty("LastModifierUserId");
            if (lastModifierUserIdProp != null && lastModifierUserIdProp.CanWrite)
            {
                // TODO: Implement current user ID retrieval from authentication system
                // lastModifierUserIdProp.SetValue(entity, currentUserId);
            }
        }

        /// <summary>
        /// Attempts to perform a soft delete on an entity by setting IsDeleted to true.
        /// </summary>
        /// <param name="entity">The entity to soft delete.</param>
        /// <returns>
        /// true if the entity has an IsDeleted property and soft delete was performed;
        /// otherwise, false.
        /// </returns>
        /// <remarks>
        /// If the entity has a DeletionTime property, it will also be set to current UTC time.
        /// If the entity has a DeleterUserId property, it should be set to the current user's ID.
        /// This method uses reflection to check for and set properties dynamically.
        /// </remarks>
        private bool TrySoftDelete(TEntity entity)
        {
            // Check if entity has IsDeleted property
            var isDeletedProp = entity.GetType().GetProperty("IsDeleted");
            if (isDeletedProp != null && isDeletedProp.CanWrite)
            {
                isDeletedProp.SetValue(entity, true);

                // Set deletion time if property exists
                var deletionTimeProp = entity.GetType().GetProperty("DeletionTime");
                if (deletionTimeProp != null && deletionTimeProp.CanWrite)
                {
                    deletionTimeProp.SetValue(entity, DateTime.UtcNow);
                }

                // Set deleter user ID if property exists
                var deleterUserIdProp = entity.GetType().GetProperty("DeleterUserId");
                if (deleterUserIdProp != null && deleterUserIdProp.CanWrite)
                {
                    // TODO: Implement current user ID retrieval from authentication system
                    // deleterUserIdProp.SetValue(entity, currentUserId);
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}