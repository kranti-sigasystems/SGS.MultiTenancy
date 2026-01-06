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
    /// Provides business logic and operations for managing countries.
    /// Implements validation, uniqueness checks, and status management.
    /// </summary>
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryService"/> class.
        /// </summary>
        /// <param name="countryRepository">The country repository dependency.</param>
        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        /// <summary>
        /// Retrieves a country by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the country.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <see cref="Country"/> if found; otherwise, <c>null</c>.
        /// </returns>
        public async Task<Country?> GetCountryAsync(Guid id)
        {
            return await _countryRepository.GetAsync(id);
        }

        /// <summary>
        /// Retrieves a country by its unique country code.
        /// </summary>
        /// <param name="code">The unique country code (e.g., ISO code).</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <see cref="Country"/> if found; otherwise, <c>null</c>.
        /// </returns>
        public async Task<Country?> GetCountryByCodeAsync(string code)
        {
            return await _countryRepository.GetByCodeAsync(code);
        }

        /// <summary>
        /// Retrieves all countries.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of <see cref="Country"/> entities.
        /// </returns>
        public async Task<List<Country>> GetAllCountriesAsync()
        {
            return await _countryRepository.GetAllListAsync();
        }

        /// <summary>
        /// Retrieves all active countries.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of active <see cref="Country"/> entities.
        /// </returns>
        public async Task<List<Country>> GetActiveCountriesAsync()
        {
            return await _countryRepository.GetActiveCountriesAsync();
        }

        /// <summary>
        /// Creates a new country after validating business rules and uniqueness constraints.
        /// </summary>
        /// <param name="country">The country entity to create.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the newly created <see cref="Country"/>.
        /// </returns>
        /// <exception cref="ValidationException">
        /// Thrown when country data is invalid or violates uniqueness constraints.
        /// </exception>
        public async Task<Country> CreateCountryAsync(Country country)
        {
            if (!ValidateCountryAsync(country))
            {
                throw new ValidationException("Invalid country data");
            }

            if (!await _countryRepository.IsCodeUniqueAsync(country.Code))
            {
                throw new ValidationException($"Country code '{country.Code}' already exists");
            }

            if (!await _countryRepository.IsNameUniqueAsync(country.Name))
            {
                throw new ValidationException($"Country name '{country.Name}' already exists");
            }

            country.ID = Guid.NewGuid();
            country.Status = EntityStatus.Active;

            return await _countryRepository.InsertAsync(country);
        }

        /// <summary>
        /// Updates an existing country.
        /// </summary>
        /// <param name="id">The unique identifier of the country to update.</param>
        /// <param name="country">The updated country details.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the updated <see cref="Country"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the specified country does not exist.
        /// </exception>
        /// <exception cref="ValidationException">
        /// Thrown when country data is invalid or violates uniqueness constraints.
        /// </exception>
        public async Task<Country> UpdateCountryAsync(Guid id, Country country)
        {
            var existingCountry = await _countryRepository.GetAsync(id);
            if (existingCountry == null)
            {
                throw new KeyNotFoundException($"Country with ID {id} not found");
            }

            if (!ValidateCountryAsync(country))
            {
                throw new ValidationException("Invalid country data");
            }

            if (!await _countryRepository.IsCodeUniqueAsync(country.Code, id))
            {
                throw new ValidationException($"Country code '{country.Code}' already exists");
            }

            if (!await _countryRepository.IsNameUniqueAsync(country.Name, id))
            {
                throw new ValidationException($"Country name '{country.Name}' already exists");
            }

            existingCountry.Name = country.Name;
            existingCountry.Code = country.Code;
            existingCountry.Status = country.Status;

            return await _countryRepository.UpdateAsync(existingCountry);
        }

        /// <summary>
        /// Deletes a country by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the country to delete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        public async Task DeleteCountryAsync(Guid id)
        {
            await _countryRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Changes the status of a country.
        /// </summary>
        /// <param name="id">The unique identifier of the country.</param>
        /// <param name="status">The new status to assign.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the updated <see cref="Country"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the specified country does not exist.
        /// </exception>
        public async Task<Country> ChangeCountryStatusAsync(Guid id, EntityStatus status)
        {
            var country = await _countryRepository.GetAsync(id);
            if (country == null)
            {
                throw new KeyNotFoundException($"Country with ID {id} not found");
            }

            country.Status = status;
            return await _countryRepository.UpdateAsync(country);
        }

        /// <summary>
        /// Validates country data based on business rules.
        /// </summary>
        /// <param name="country">The country entity to validate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result is <c>true</c> if the country data is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool ValidateCountryAsync(Country country)
        {
            if (string.IsNullOrWhiteSpace(country.Name) ||
                string.IsNullOrWhiteSpace(country.Code))
            {
                return false;
            }

            if (country.Name.Length > 50)
            {
                return false;
            }

            if (country.Code.Length > 5)
            {
                return false;
            }

            return true;
        }
    }
}
