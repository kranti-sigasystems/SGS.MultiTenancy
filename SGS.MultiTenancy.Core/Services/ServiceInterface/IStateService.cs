using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    /// <summary>
    /// Defines state-related business operations.
    /// Provides methods for managing states and their association with countries.
    /// </summary>
    public interface IStateService
    {
        /// <summary>
        /// Retrieves a state by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the state.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <see cref="State"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<State?> GetStateAsync(Guid id);

        /// <summary>
        /// Retrieves a state by its unique identifier along with its associated country.
        /// </summary>
        /// <param name="id">The unique identifier of the state.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <see cref="State"/> including country details,
        /// or <c>null</c> if not found.
        /// </returns>
        Task<State?> GetStateWithCountryAsync(Guid id);

        /// <summary>
        /// Retrieves all states.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of <see cref="State"/> entities.
        /// </returns>
        Task<List<State>> GetAllStatesAsync();

        /// <summary>
        /// Retrieves all states for a specific country by country identifier.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of <see cref="State"/> entities.
        /// </returns>
        Task<List<State>> GetStatesByCountryIdAsync(Guid countryId);

        /// <summary>
        /// Retrieves all states for a specific country using the country code.
        /// </summary>
        /// <param name="countryCode">The unique country code (e.g., ISO code).</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of <see cref="State"/> entities.
        /// </returns>
        Task<List<State>> GetStatesByCountryCodeAsync(string countryCode);

        /// <summary>
        /// Retrieves all active states.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of active <see cref="State"/> entities.
        /// </returns>
        Task<List<State>> GetActiveStatesAsync();

        /// <summary>
        /// Creates a new state.
        /// </summary>
        /// <param name="state">The state entity to create.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the newly created <see cref="State"/>.
        /// </returns>
        Task<State> CreateStateAsync(State state);

        /// <summary>
        /// Updates an existing state.
        /// </summary>
        /// <param name="id">The unique identifier of the state to update.</param>
        /// <param name="state">The updated state details.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the updated <see cref="State"/>.
        /// </returns>
        Task<State> UpdateStateAsync(Guid id, State state);

        /// <summary>
        /// Deletes a state by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the state to delete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        Task DeleteStateAsync(Guid id);

        /// <summary>
        /// Changes the status of a state.
        /// </summary>
        /// <param name="id">The unique identifier of the state.</param>
        /// <param name="status">The new status to assign.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the updated <see cref="State"/>.
        /// </returns>
        Task<State> ChangeStateStatusAsync(Guid id, EntityStatus status);

        /// <summary>
        /// Validates the given state based on business rules.
        /// </summary>
        /// <param name="state">The state entity to validate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result is <c>true</c> if the state is valid; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> ValidateStateAsync(State state);

        /// <summary>
        /// Checks whether a state name is unique within a specific country.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country.</param>
        /// <param name="stateName">The state name to validate.</param>
        /// <param name="excludeId">
        /// An optional state identifier to exclude from the uniqueness check
        /// (useful during update operations).
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result is <c>true</c> if the state name is unique within the country;
        /// otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsStateNameUniqueInCountryAsync(Guid countryId, string stateName, Guid? excludeId = null);
    }
}
