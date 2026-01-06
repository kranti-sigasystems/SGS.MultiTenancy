using SGS.MultiTenancy.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    /// <summary>
    /// Repository interface for managing <see cref="State"/> entities with state-specific operations.
    /// Extends the basic CRUD operations provided by <see cref="IGenericRepository{T}"/>.
    /// </summary>
    public interface IStateRepository : IGenericRepository<State>
    {
        /// <summary>
        /// Retrieves a <see cref="State"/> entity by its unique identifier, including its associated country information.
        /// </summary>
        /// <param name="id">The unique identifier of the state to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="State"/> 
        /// with country details if found; otherwise, <c>null</c>.
        /// </returns>
        Task<State?> GetWithCountryAsync(Guid id);

        /// <summary>
        /// Retrieves a list of <see cref="State"/> entities that belong to a specific country.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country to filter states by.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of 
        /// <see cref="State"/> entities belonging to the specified country.
        /// </returns>
        Task<List<State>> GetByCountryIdAsync(Guid countryId);

        /// <summary>
        /// Retrieves a list of <see cref="State"/> entities that belong to a specific country by its code.
        /// </summary>
        /// <param name="countryCode">The code of the country to filter states by (e.g., "US", "IN").</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of 
        /// <see cref="State"/> entities belonging to the specified country code.
        /// </returns>
        Task<List<State>> GetByCountryCodeAsync(string countryCode);

        /// <summary>
        /// Retrieves a list of active <see cref="State"/> entities.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of 
        /// active <see cref="State"/> entities.
        /// </returns>
        Task<List<State>> GetActiveStatesAsync();

        /// <summary>
        /// Checks whether a state with the specified name already exists within a given country.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country to check within.</param>
        /// <param name="stateName">The name of the state to check for existence.</param>
        /// <param name="excludeId">Optional unique identifier of a state to exclude from the check (useful for update operations).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is <c>true</c> if a state 
        /// with the specified name exists in the country (excluding the specified state if provided); otherwise, <c>false</c>.
        /// </returns>
        Task<bool> ExistsInCountryAsync(Guid countryId, string stateName, Guid? excludeId = null);
    }
}