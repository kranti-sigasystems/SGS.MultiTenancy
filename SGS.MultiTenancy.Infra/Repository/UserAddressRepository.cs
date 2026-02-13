using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Application.Interfaces.Repositories;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Infra.DataContext;

namespace SGS.MultiTenancy.Infra.Repository
{
    /// <summary>
    /// Concrete repository for user-address mappings.
    /// </summary>
    public class UserAddressRepository : GenericRepository<UserAddress>, IUserAddressRepository
    {
        private readonly AppDbContext _context;

        public UserAddressRepository(AppDbContext context, ICurrentUser? currentUser = null)
            : base(context, currentUser)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all user addresses associated with the specified user identifier.
        /// </summary>
        /// <param name="userId"></param>
        public async Task<List<UserAddress>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserAddresses.Where(ua => ua.UserID == userId).Include(ua => ua.Address).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Retrieves a user address mapping based on user and address identifiers.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="addressId"></param>
        public async Task<UserAddress?> GetByUserAndAddressAsync(Guid userId, Guid addressId)
        {
            return await _context.UserAddresses.Include(ua => ua.Address).FirstOrDefaultAsync(ua => ua.UserID == userId && ua.AddressId == addressId);
        }
    }
}