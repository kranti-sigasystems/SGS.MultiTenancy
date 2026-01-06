using SGS.MultiTenancy.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    /// <summary>
    /// Defines address-related business operations.
    /// Provides methods for managing, validating, and searching addresses.
    /// </summary>
    public interface IAddressService
    {
        /// <summary>
        /// Retrieves an address by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the address.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <see cref="Address"/> if found;
        /// otherwise, <c>null</c>.
        /// </returns>
        Task<Address?> GetAddressAsync(Guid id);

        /// <summary>
        /// Retrieves all addresses.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of <see cref="Address"/> entities.
        /// </returns>
        Task<List<Address>> GetAllAddressesAsync();

        /// <summary>
        /// Creates a new address.
        /// </summary>
        /// <param name="address">The address entity to create.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the newly created <see cref="Address"/>.
        /// </returns>
        Task<Address> CreateAddressAsync(Address address);

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="id">The unique identifier of the address to update.</param>
        /// <param name="address">The updated address details.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the updated <see cref="Address"/>.
        /// </returns>
        Task<Address> UpdateAddressAsync(Guid id, Address address);

        /// <summary>
        /// Deletes an address by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the address to delete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        Task DeleteAddressAsync(Guid id);

        /// <summary>
        /// Searches addresses based on city and postal code.
        /// </summary>
        /// <param name="city">The city name to filter addresses.</param>
        /// <param name="postalCode">The postal code to filter addresses.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of matching <see cref="Address"/> entities.
        /// </returns>
        List<Address> SearchAddresses(string city, string postalCode);

        /// <summary>
        /// Validates the given address based on business rules.
        /// </summary>
        /// <param name="address">The address entity to validate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result is <c>true</c> if the address is valid; otherwise, <c>false</c>.
        /// </returns>
        bool ValidateAddressAsync(Address address);
    }
}
