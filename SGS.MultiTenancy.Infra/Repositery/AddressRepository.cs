using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Entities.Common;
using SGS.MultiTenancy.Infra.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Infra.Repositery
{
    /// <summary>
    /// Repository implementation for managing <see cref="Address"/> entity data access operations.
    /// Provides address-specific data access methods in addition to the generic CRUD operations.
    /// </summary>
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
        public AddressRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves an <see cref="Address"/> entity by its unique identifier, including all related details.
        /// </summary>
        /// <param name="id">The unique identifier of the address to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="Address"/> 
        /// with all related details if found; otherwise, <c>null</c>.
        /// </returns>
        /// <remarks>
        /// This method currently returns the address entity without including related entities.
        /// If the Address entity has navigation properties that need to be included, 
        /// consider adding <c>.Include()</c> calls to load related data.
        /// </remarks>
        public async Task<Address?> GetWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.ID == id);
        }

        /// <summary>
        /// Retrieves a list of <see cref="Address"/> entities that belong to the specified city.
        /// </summary>
        /// <param name="city">The name of the city to filter addresses by.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of 
        /// <see cref="Address"/> entities matching the specified city (case-insensitive).
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="city"/> is null or whitespace.</exception>
        public async Task<List<Address>> GetByCityAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be null or empty", nameof(city));

            return await _dbSet
                .Where(a => a.City.ToLower() == city.ToLower())
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a list of <see cref="Address"/> entities that match the specified postal code.
        /// </summary>
        /// <param name="postalCode">The postal code to filter addresses by.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of 
        /// <see cref="Address"/> entities matching the specified postal code.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="postalCode"/> is null or whitespace.</exception>
        public async Task<List<Address>> GetByPostalCodeAsync(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
                throw new ArgumentException("Postal code cannot be null or empty", nameof(postalCode));

            return await _dbSet
                .Where(a => a.PostalCode == postalCode)
                .ToListAsync();
        }
    }
}