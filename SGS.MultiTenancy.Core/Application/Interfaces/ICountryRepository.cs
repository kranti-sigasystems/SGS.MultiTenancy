using SGS.MultiTenancy.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    /// <summary>
    /// Defines country-specific data access operations.
    /// Extends the generic repository with additional queries
    /// related to <see cref="Country"/> entities.
    /// </summary>
    public interface ICountryRepository : IGenericRepository<Country>
    {
        /// <summary>
        /// Retrieves a country by its unique country code.
        /// </summary>
        /// <param name="code">The unique country code (e.g., ISO code).</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the matching <see cref="Country"/> entity,
        /// or <c>null</c> if no country is found.
        /// </returns>
        Task<Country?> GetByCodeAsync(string code);

        /// <summary>
        /// Retrieves all active countries.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of active <see cref="Country"/> entities.
        /// </returns>
        Task<List<Country>> GetActiveCountriesAsync();

        /// <summary>
        /// Checks whether a country code is unique.
        /// </summary>
        /// <param name="code">The country code to validate.</param>
        /// <param name="excludeId">
        /// An optional country identifier to exclude from the uniqueness check
        /// (useful during update operations).
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result is <c>true</c> if the code is unique; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsCodeUniqueAsync(string code, Guid? excludeId = null);

        /// <summary>
        /// Checks whether a country name is unique.
        /// </summary>
        /// <param name="name">The country name to validate.</param>
        /// <param name="excludeId">
        /// An optional country identifier to exclude from the uniqueness check
        /// (useful during update operations).
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result is <c>true</c> if the name is unique; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null);
    }
}