using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Entities.Common;

namespace SGS.MultiTenancy.Core.Application.Interfaces.Repositories
{
    /// <summary>
    /// Contract for address repository.
    /// </summary>
    public interface IAddressRepository : IGenericRepository<Address>
    {
        /// <summary>
        /// Gets all addresses for a tenant.
        /// </summary>
        Task<List<Address>> GetByTenantAsync(Guid tenantId);

        /// <summary>
        /// Gets all addresses associated with a user.
        /// </summary>
        Task<List<Address>> GetByUserIdAsync(Guid userId);
    }
}