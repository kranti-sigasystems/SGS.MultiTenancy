using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Entities.Common;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Application.Services
{
    /// <summary>
    /// Service implementation for managing address-related business operations.
    /// Provides methods for creating, retrieving, updating, deleting, and validating addresses.
    /// </summary>
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressService"/> class.
        /// </summary>
        /// <param name="addressRepository">The repository for address data access operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="addressRepository"/> is null.</exception>
        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        }

        /// <summary>
        /// Retrieves an address by its unique identifier with full details.
        /// </summary>
        /// <param name="id">The unique identifier of the address to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="Address"/> 
        /// with details if found; otherwise, <c>null</c>.
        /// </returns>
        public async Task<Address?> GetAddressAsync(Guid id)
        {
            return await _addressRepository.GetWithDetailsAsync(id);
        }

        /// <summary>
        /// Retrieves all addresses from the system.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of all 
        /// <see cref="Address"/> entities.
        /// </returns>
        public async Task<List<Address>> GetAllAddressesAsync()
        {
            return await _addressRepository.GetAllListAsync();
        }

        /// <summary>
        /// Creates a new address in the system.
        /// </summary>
        /// <param name="address">The address entity to create.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the created 
        /// <see cref="Address"/> entity.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="address"/> is null.</exception>
        /// <exception cref="ValidationException">Thrown when the address fails validation.</exception>
        public async Task<Address> CreateAddressAsync(Address address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            if (!ValidateAddressAsync(address))
            {
                throw new ValidationException("Invalid address data");
            }

            address.ID = Guid.NewGuid();
            return await _addressRepository.InsertAsync(address);
        }

        /// <summary>
        /// Updates an existing address with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the address to update.</param>
        /// <param name="address">The address entity containing the updated data.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the updated 
        /// <see cref="Address"/> entity.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="address"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when no address exists with the specified ID.</exception>
        /// <exception cref="ValidationException">Thrown when the address fails validation.</exception>
        public async Task<Address> UpdateAddressAsync(Guid id, Address address)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Address ID cannot be empty", nameof(id));

            if (address == null)
                throw new ArgumentNullException(nameof(address));

            var existingAddress = await _addressRepository.GetAsync(id);
            if (existingAddress == null)
            {
                throw new KeyNotFoundException($"Address with ID {id} not found");
            }

            if (!ValidateAddressAsync(address))
            {
                throw new ValidationException("Invalid address data");
            }

            // Update properties
            existingAddress.AddressLine = address.AddressLine;
            existingAddress.PostalCode = address.PostalCode;
            existingAddress.City = address.City;

            return await _addressRepository.UpdateAsync(existingAddress);
        }

        /// <summary>
        /// Deletes an address from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the address to delete.</param>
        /// <returns>
        /// A task that represents the asynchronous delete operation.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is empty.</exception>
        public async Task DeleteAddressAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Address ID cannot be empty", nameof(id));

            await _addressRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Searches for addresses based on city and postal code criteria.
        /// </summary>
        /// <param name="city">The city name to search for (case-insensitive partial match).</param>
        /// <param name="postalCode">The postal code to search for (exact match).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of 
        /// <see cref="Address"/> entities matching the search criteria.
        /// </returns>
        public List<Address> SearchAddresses(string city, string postalCode)
        {
            var query = _addressRepository.GetAll();

            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(a => a.City.ToLower().Contains(city.ToLower()));
            }

            if (!string.IsNullOrEmpty(postalCode))
            {
                query = query.Where(a => a.PostalCode.Contains(postalCode));
            }

            return query.ToList();
        }

        /// <summary>
        /// Validates an address entity to ensure it meets business rules and data integrity requirements.
        /// </summary>
        /// <param name="address">The address entity to validate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is <c>true</c> if the 
        /// address is valid according to business rules; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Validation includes checking for required fields and field length constraints:
        /// - AddressLine: Required, maximum 250 characters
        /// - PostalCode: Required, maximum 10 characters
        /// - City: Required
        /// </remarks>
        public bool ValidateAddressAsync(Address address)
        {
            if (address == null)
                return false;

            if (string.IsNullOrWhiteSpace(address.AddressLine) ||
                string.IsNullOrWhiteSpace(address.PostalCode) ||
                string.IsNullOrWhiteSpace(address.City))
            {
                return false;
            }

            if (address.AddressLine.Length > 250)
            {
                return false;
            }

            if (address.PostalCode.Length > 10)
            {
                return false;
            }

            return true;
        }
    }
}