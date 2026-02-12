using SGS.MultiTenancy.Core.Domain.Entities.Auth;

namespace SGS.MultiTenancy.Core.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository contract for managing user-address associations.
    /// </summary>
    public interface IUserAddressRepository : IGenericRepository<UserAddress>
    {
        /// <summary>
        /// Gets all user-address mappings for the specified user (includes Address).
        /// </summary>
        Task<List<UserAddress>> GetByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets a specific user-address mapping by user and address identifiers.
        /// </summary>
        Task<UserAddress?> GetByUserAndAddressAsync(Guid userId, Guid addressId);
    }
}