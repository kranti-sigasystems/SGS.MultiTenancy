using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Core.Services
{
    /// <summary>
    /// Provides business logic and operations for managing states.
    /// Handles validation, country association, uniqueness constraints,
    /// and status management.
    /// </summary>
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateService"/> class.
        /// </summary>
        /// <param name="stateRepository">The state repository dependency.</param>
        /// <param name="countryRepository">The country repository dependency.</param>
        public StateService(
            IStateRepository stateRepository,
            ICountryRepository countryRepository)
        {
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
        }

        /// <summary>
        /// Retrieves a state by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the state.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <see cref="State"/> if found;
        /// otherwise, <c>null</c>.
        /// </returns>
        public async Task<State?> GetStateAsync(Guid id)
        {
            return await _stateRepository.GetAsync(id);
        }

        /// <summary>
        /// Retrieves a state along with its associated country.
        /// </summary>
        /// <param name="id">The unique identifier of the state.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <see cref="State"/> including country details,
        /// or <c>null</c> if not found.
        /// </returns>
        public async Task<State?> GetStateWithCountryAsync(Guid id)
        {
            return await _stateRepository.GetWithCountryAsync(id);
        }

        /// <summary>
        /// Retrieves all states.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of <see cref="State"/> entities.
        /// </returns>
        public async Task<List<State>> GetAllStatesAsync()
        {
            return await _stateRepository.GetAllListAsync();
        }

        /// <summary>
        /// Retrieves all states for a specific country by country identifier.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of <see cref="State"/> entities.
        /// </returns>
        public async Task<List<State>> GetStatesByCountryIdAsync(Guid countryId)
        {
            return await _stateRepository.GetByCountryIdAsync(countryId);
        }

        /// <summary>
        /// Retrieves all states for a specific country using the country code.
        /// </summary>
        /// <param name="countryCode">The unique country code (e.g., ISO code).</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of <see cref="State"/> entities.
        /// </returns>
        public async Task<List<State>> GetStatesByCountryCodeAsync(string countryCode)
        {
            return await _stateRepository.GetByCountryCodeAsync(countryCode);
        }

        /// <summary>
        /// Retrieves all active states.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of active <see cref="State"/> entities.
        /// </returns>
        public async Task<List<State>> GetActiveStatesAsync()
        {
            return await _stateRepository.GetActiveStatesAsync();
        }

        /// <summary>
        /// Creates a new state after validating business rules and country association.
        /// </summary>
        /// <param name="state">The state entity to create.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the newly created <see cref="State"/>.
        /// </returns>
        /// <exception cref="ValidationException">
        /// Thrown when state data is invalid or violates uniqueness constraints.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the associated country does not exist.
        /// </exception>
        public async Task<State> CreateStateAsync(State state)
        {
            if (!await ValidateStateAsync(state))
            {
                throw new ValidationException("Invalid state data");
            }

            var country = await _countryRepository.GetAsync(state.CountryID);
            if (country == null)
            {
                throw new KeyNotFoundException($"Country with ID {state.CountryID} not found");
            }

            if (!await IsStateNameUniqueInCountryAsync(state.CountryID, state.Name))
            {
                throw new ValidationException(
                    $"State name '{state.Name}' already exists in this country");
            }

            state.ID = Guid.NewGuid();
            state.Status = EntityStatus.Active;

            return await _stateRepository.InsertAsync(state);
        }

        /// <summary>
        /// Updates an existing state.
        /// </summary>
        /// <param name="id">The unique identifier of the state to update.</param>
        /// <param name="state">The updated state details.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the updated <see cref="State"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the state or associated country does not exist.
        /// </exception>
        /// <exception cref="ValidationException">
        /// Thrown when state data is invalid or violates uniqueness constraints.
        /// </exception>
        public async Task<State> UpdateStateAsync(Guid id, State state)
        {
            var existingState = await _stateRepository.GetAsync(id);
            if (existingState == null)
            {
                throw new KeyNotFoundException($"State with ID {id} not found");
            }

            if (!await ValidateStateAsync(state))
            {
                throw new ValidationException("Invalid state data");
            }

            var country = await _countryRepository.GetAsync(state.CountryID);
            if (country == null)
            {
                throw new KeyNotFoundException($"Country with ID {state.CountryID} not found");
            }

            if (!await IsStateNameUniqueInCountryAsync(state.CountryID, state.Name, id))
            {
                throw new ValidationException(
                    $"State name '{state.Name}' already exists in this country");
            }

            existingState.Name = state.Name;
            existingState.Code = state.Code;
            existingState.CountryID = state.CountryID;
            existingState.Status = state.Status;

            return await _stateRepository.UpdateAsync(existingState);
        }

        /// <summary>
        /// Deletes a state by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the state to delete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        public async Task DeleteStateAsync(Guid id)
        {
            await _stateRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Changes the status of a state.
        /// </summary>
        /// <param name="id">The unique identifier of the state.</param>
        /// <param name="status">The new status to assign.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the updated <see cref="State"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the specified state does not exist.
        /// </exception>
        public async Task<State> ChangeStateStatusAsync(Guid id, EntityStatus status)
        {
            var state = await _stateRepository.GetAsync(id);
            if (state == null)
            {
                throw new KeyNotFoundException($"State with ID {id} not found");
            }

            state.Status = status;
            return await _stateRepository.UpdateAsync(state);
        }

        /// <summary>
        /// Validates state data based on business rules.
        /// </summary>
        /// <param name="state">The state entity to validate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result is <c>true</c> if the state data is valid; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> ValidateStateAsync(State state)
        {
            if (string.IsNullOrWhiteSpace(state.Name) ||
                state.CountryID == Guid.Empty)
            {
                return false;
            }

            if (state.Name.Length > 50)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(state.Code) && state.Code.Length > 10)
            {
                return false;
            }

            return true;
        }

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
        public async Task<bool> IsStateNameUniqueInCountryAsync(
            Guid countryId,
            string stateName,
            Guid? excludeId = null)
        {
            return !await _stateRepository
                .ExistsInCountryAsync(countryId, stateName, excludeId);
        }
    }
}
