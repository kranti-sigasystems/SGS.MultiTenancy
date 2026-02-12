using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Application.Interfaces.Repositories;
using SGS.MultiTenancy.Core.Entities.Common;
using SGS.MultiTenancy.Infra.DataContext;

namespace SGS.MultiTenancy.Infra.Repository
{
    /// <summary>
    /// Repository for managing addresses.
    /// </summary>
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context, ICurrentUser? currentUser = null)
            : base(context, currentUser)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all addresses associated with the specified tenant.
        /// </summary>
        /// <param name="tenantId"></param>
        public async Task<List<Address>> GetByTenantAsync(Guid tenantId)
        {
            return await _context.Address .Where(a => a.TenantID == tenantId).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Retrieves all addresses associated with the specified user identifier.
        /// </summary>
        /// <param name="userId"></param>
        public async Task<List<Address>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserAddresses.Where(ua => ua.UserID == userId).Include(ua => ua.Address).Select(ua => ua.Address).AsNoTracking() .ToListAsync();
        }
    }
}