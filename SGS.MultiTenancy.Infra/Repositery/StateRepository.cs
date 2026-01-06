using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Infra.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Infra.Repositery
{
    /// <summary>
    /// Repository implementation for <see cref="State"/> entities.
    /// Provides Entity Framework Core–based data access operations
    /// including country associations and status filtering.
    /// </summary>
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public StateRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a state by its unique identifier including its associated country.
        /// </summary>
        /// <param name="id">The unique identifier of the state.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <see cref="State"/> with country details,
        /// or <c>null</c> if not found.
        /// </returns>
        public async Task<State?> GetWithCountryAsync(Guid id)
        {
            return await _dbSet
                .Include(s => s.Country)
                .FirstOrDefaultAsync(s => s.ID == id);
        }

        /// <summary>
        /// Retrieves all states belonging to a specific country by country identifier.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of <see cref="State"/> entities
        /// ordered by name.
        /// </returns>
        public async Task<List<State>> GetByCountryIdAsync(Guid countryId)
        {
            return await _dbSet
                .Include(s => s.Country)
                .Where(s => s.CountryID == countryId)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all states belonging to a specific country using the country code.
        /// </summary>
        /// <param name="countryCode">The unique country code (e.g., ISO code).</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of <see cref="State"/> entities
        /// ordered by name.
        /// </returns>
        public async Task<List<State>> GetByCountryCodeAsync(string countryCode)
        {
            return await _dbSet
                .Include(s => s.Country)
                .Where(s => s.Country.Code.ToLower() == countryCode.ToLower())
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all active states whose associated countries are also active.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of active <see cref="State"/> entities
        /// ordered by name.
        /// </returns>
        public async Task<List<State>> GetActiveStatesAsync()
        {
            return await _dbSet
                .Include(s => s.Country)
                .Where(s => s.Status == EntityStatus.Active &&
                            s.Country.Status == EntityStatus.Active)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Checks whether a state with the given name already exists
        /// within a specific country.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country.</param>
        /// <param name="stateName">The state name to check.</param>
        /// <param name="excludeId">
        /// An optional state identifier to exclude from the check
        /// (useful during update operations).
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result is <c>true</c> if a matching state exists;
        /// otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> ExistsInCountryAsync(
            Guid countryId,
            string stateName,
            Guid? excludeId = null)
        {
            var query = _dbSet
                .Where(s => s.CountryID == countryId &&
                            s.Name.ToLower() == stateName.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(s => s.ID != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
