using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
namespace SGS.MultiTenancy.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepositery;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IGenericRepository<UserRoles> _userRoles;
        private readonly IMemoryCache _cache;
        public UserService(IUserRepository userRepositery,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasherService passwordHasherService,
            IGenericRepository<UserRoles> userRoles,
            IMemoryCache memoryCache)
        {
            _userRepositery = userRepositery;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasherService = passwordHasherService;
            _userRoles = userRoles;
            _cache = memoryCache;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        { 
            User? user = await _userRepositery.FirstOrDefaultAsync(
            x => x.Name.ToLower() == loginRequestDto.UserName.ToLower(),
            query => query.Include(u => u.UserRoles));
           
            bool isPasswordValid = _passwordHasherService.VerifyPassword(loginRequestDto.Password, user.Password);

            if (!isPasswordValid ||user == null || !user.UserRoles.Any())
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = " ",
                };
            }

            List<Guid> roleIds = user.UserRoles
                          .Select(ur => ur.RoleID)
                          .Distinct()
                          .ToList();

            List<Role> roles = await _userRepositery.GetRolesWithPermissionsAsync(roleIds);

            List<string> userRoles = roles
                .Select(r => r.Name)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .ToList();

            string token = _jwtTokenGenerator.GenerateToken(user, userRoles);
            UserDto userDTO = new()
            {
                Email = user.Email,
                ID = user.ID,
                UserName = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto Response = new LoginResponseDto()
            {
                User = userDTO,
                Token = token,
                Roles = userRoles,
                TenantID = user.TenantID
            };

            return Response;
        }

        public async Task<bool> UserHasPermissionAsync(Guid userId, Guid tenantId, Guid permissionId)
        {
            HashSet <Guid> permissionIds = await GetUserPermissionIdsAsync(userId, tenantId);
            return permissionIds.Contains(permissionId);
        }

        private async Task<HashSet<Guid>> GetUserPermissionIdsAsync(
            Guid userId,
            Guid tenantId)
        {
            string cacheKey = $"perm:{tenantId}:{userId}";

            if (_cache.TryGetValue(cacheKey, out HashSet<Guid> cached))
                return cached;

            List<Guid> permissionIds = await _userRoles.Query()
                .Where(ur =>
                    ur.UserID == userId &&
                    ur.TenantID == tenantId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.PermissionID)
                .Distinct()
                .ToListAsync();

            var result = permissionIds.ToHashSet();

            _cache.Set(
                cacheKey,
                result,
                TimeSpan.FromMinutes(30));

            return result;
        }
    }
}