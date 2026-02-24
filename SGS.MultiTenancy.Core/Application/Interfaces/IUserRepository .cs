using SGS.MultiTenancy.Core.Domain.Entities.Auth;

namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        /// <summary>
        /// Return all the roles based on the Identifiers
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns>Returns all the UserRoles</returns>
        public Task<List<Role>> GetRolesWithPermissionsAsync(IEnumerable<Guid> roleIds);

        /// <summary>
        /// Return a user by the identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<User?> GetUserByIdAsync(Guid id);
    }
}