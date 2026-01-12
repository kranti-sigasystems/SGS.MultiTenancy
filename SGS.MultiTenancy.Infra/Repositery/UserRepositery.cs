using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Infra.DataContext;

namespace SGS.MultiTenancy.Infra.Repositery
{
    public class UserRepository : GenericRepository<User>, IUserRepositery
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the UserRepository class using the specified database context.
        /// </summary>
        /// <param name="context">The database context to be used for data access operations. Cannot be null.</param>
        public UserRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves roles by identifier including their associated permissions.
        /// </summary>
        /// <param name="roleIds">The role identifiers to filter by.</param>
        /// <returns>
        /// A list of roles with their permissions included.
        /// </returns>
        public async Task<List<Role>> GetRolesWithPermissionsAsync(IEnumerable<Guid> roleIds)
        {
            return await _context.Roles
                .Where(r => roleIds.Contains(r.ID))
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}