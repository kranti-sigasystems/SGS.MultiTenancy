using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    /// <summary>
    /// Service interface for managing country-related business operations.
    /// Provides methods for retrieving, creating, updating, and managing country entities.
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Retrieves a country by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the country to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="Country"/> 
        /// if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Country?> GetCountryAsync(Guid id);

        /// <summary>
        /// Retrieves a country by its country code.
        /// </summary>
        /// <param name="code">The country code to search for (e.g., "US", "IN", "GB").</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="Country"/> 
        /// if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Country?> GetCountryByCodeAsync(string code);

        /// <summary>
        /// Retrieves all countries from the system.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of all 
        /// <see cref="Country"/> entities.
        /// </returns>
        Task<List<Country>> GetAllCountriesAsync();

        /// <summary>
        /// Retrieves all active countries from the system.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of active 
        /// <see cref="Country"/> entities.
        /// </returns>
        Task<List<Country>> GetActiveCountriesAsync();

        /// <summary>
        /// Creates a new country in the system.
        /// </summary>
        /// <param name="country">The country entity containing the data to create.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the created 
        /// <see cref="Country"/> with any server-generated values.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="country"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when validation fails or a country with the same code already exists.</exception>
        Task<Country> CreateCountryAsync(Country country);

        /// <summary>
        /// Updates an existing country with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the country to update.</param>
        /// <param name="country">The country entity containing the updated data.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the updated 
        /// <see cref="Country"/> entity.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is empty or not found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="country"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when validation fails.</exception>
        Task<Country> UpdateCountryAsync(Guid id, Country country);

        /// <summary>
        /// Deletes a country from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the country to delete.</param>
        /// <returns>
        /// A task that represents the asynchronous delete operation.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is empty or not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the country cannot be deleted due to existing dependencies.</exception>
        Task DeleteCountryAsync(Guid id);

        /// <summary>
        /// Changes the status of a country to the specified status.
        /// </summary>
        /// <param name="id">The unique identifier of the country to update.</param>
        /// <param name="status">The new status to set for the country.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the updated 
        /// <see cref="Country"/> entity with the new status.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is empty or not found.</exception>
        Task<Country> ChangeCountryStatusAsync(Guid id, EntityStatus status);

        /// <summary>
        /// Validates a country entity to ensure it meets business rules and data integrity requirements.
        /// </summary>
        /// <param name="country">The country entity to validate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is <c>true</c> if the 
        /// country is valid according to business rules; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method typically checks for required fields, format validation, and business rule compliance.
        /// </remarks>
        bool ValidateCountryAsync(Country country);
    }
}