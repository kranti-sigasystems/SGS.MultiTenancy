using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Infra.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Infra.Repositery
{
    /// <summary>
    /// Repository implementation for managing <see cref="Country"/> entity data access operations.
    /// Provides country-specific data access methods in addition to the generic CRUD operations.
    /// </summary>
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
        public CountryRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a <see cref="Country"/> entity by its country code.
        /// </summary>
        /// <param name="code">The country code to search for (e.g., "US", "IN", "GB").</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="Country"/> 
        /// if found; otherwise, <c>null</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> is null or whitespace.</exception>
        public async Task<Country?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Country code cannot be null or empty", nameof(code));

            return await _dbSet
                .FirstOrDefaultAsync(c => c.Code.ToLower() == code.ToLower());
        }

        /// <summary>
        /// Retrieves a list of active <see cref="Country"/> entities, ordered alphabetically by name.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of 
        /// active <see cref="Country"/> entities sorted by name in ascending order.
        /// </returns>
        /// <remarks>
        /// Only countries with <see cref="EntityStatus.Active"/> status are returned.
        /// </remarks>
        public async Task<List<Country>> GetActiveCountriesAsync()
        {
            return await _dbSet
                .Where(c => c.Status == EntityStatus.Active)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Checks whether a country code is unique in the database.
        /// </summary>
        /// <param name="code">The country code to check for uniqueness (case-insensitive).</param>
        /// <param name="excludeId">Optional unique identifier of a country to exclude from the check (useful for update operations).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is <c>true</c> if the 
        /// country code is unique (or belongs to the excluded country); otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> is null or whitespace.</exception>
        /// <remarks>
        /// This method is typically used during create and update operations to ensure country code uniqueness.
        /// </remarks>
        public async Task<bool> IsCodeUniqueAsync(string code, Guid? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Country code cannot be null or empty", nameof(code));

            var query = _dbSet.Where(c => c.Code.ToLower() == code.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.ID != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        /// <summary>
        /// Checks whether a country name is unique in the database.
        /// </summary>
        /// <param name="name">The country name to check for uniqueness (case-insensitive).</param>
        /// <param name="excludeId">Optional unique identifier of a country to exclude from the check (useful for update operations).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is <c>true</c> if the 
        /// country name is unique (or belongs to the excluded country); otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or whitespace.</exception>
        /// <remarks>
        /// This method is typically used during create and update operations to ensure country name uniqueness.
        /// </remarks>
        public async Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Country name cannot be null or empty", nameof(name));

            var query = _dbSet.Where(c => c.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.ID != excludeId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}