using SGS.MultiTenancy.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    /// <summary>
    /// Repository interface for managing <see cref="Address"/> entities with additional address-specific operations.
    /// Extends the basic CRUD operations provided by <see cref="IGenericRepository{T}"/>.
    /// </summary>
    public interface IAddressRepository : IGenericRepository<Address>
    {
        /// <summary>
        /// Retrieves an <see cref="Address"/> entity by its unique identifier, including all related details.
        /// </summary>
        /// <param name="id">The unique identifier of the address to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="Address"/> 
        /// with all related details if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Address?> GetWithDetailsAsync(Guid id);

        /// <summary>
        /// Retrieves a list of <see cref="Address"/> entities that belong to the specified city.
        /// </summary>
        /// <param name="city">The name of the city to filter addresses by.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of 
        /// <see cref="Address"/> entities matching the specified city.
        /// </returns>
        Task<List<Address>> GetByCityAsync(string city);

        /// <summary>
        /// Retrieves a list of <see cref="Address"/> entities that match the specified postal code.
        /// </summary>
        /// <param name="postalCode">The postal code to filter addresses by.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of 
        /// <see cref="Address"/> entities matching the specified postal code.
        /// </returns>
        Task<List<Address>> GetByPostalCodeAsync(string postalCode);
    }
}