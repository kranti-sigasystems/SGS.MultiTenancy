using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
namespace SGS.MultiTenancy.Core.Services
{
    public class UserService : IUserService
    {
        /// <summary>
        /// Creates a new user service instance.
        /// </summary>
        /// <param name="userRepositery">User data repository.</param>
        /// <param name="jwtTokenGenerator">JWT generator.</param>
        /// <param name="passwordHasherService">Password hasher.</param>
       
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
        /// <summary>
        /// Validates credentials and returns JWT with roles and permissions.
        /// </summary>
        /// <param name="loginRequestDto">Login request.</param>
        /// <returns>Login response with user info and token.</returns>
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        { 
            User? user = await _userRepositery.FirstOrDefaultAsync(
            x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower(),
            query => query.Include(u => u.UserRoles));

            if (user == null || !user.UserRoles.Any()
                || !_passwordHasherService.VerifyPassword(loginRequestDto.Password, user.PasswordHash))
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
                UserName = user.UserName
            };

            LoginResponseDto LoginResponse = new LoginResponseDto()
            {
                User = userDTO,
                Token = token,
                Roles = userRoles,
                TenantID = (Guid)user.TenantID
            };
            return LoginResponse;
        }

        ///<summary>
        /// Updates user password after verifying current one.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="currentPassword">Current password.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>Success flag and error message if any.</returns>
        public async Task<(bool Success, string ErrorMessage)> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            User? user = await _userRepositery.FirstOrDefaultAsync(u => u.ID == userId);
           
            if (user == null)
            {
                return (false, Constants.UserNotFound);
            }
            
            if (!_passwordHasherService.VerifyPassword(currentPassword, user.PasswordHash))
            {
                return (false, Constants.CurrentPasswordIncorrect);
            }
            string newPasswordHash = _passwordHasherService.HashPassword(newPassword);
            user.PasswordHash = newPasswordHash;

            await _userRepositery.UpdateAsync(user);
            await _userRepositery.CompleteAsync();

            return (true, string.Empty);
        }

        /// <summary>
        /// Handles forgot password request.
        /// Generates reset token and sends reset instructions.
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>Always returns true (prevents user enumeration)</returns>
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

            User? user = await _userRepositery.FirstOrDefaultAsync(
                u => u.Email.ToLower() == email.ToLower());

            // SECURITY: do not reveal if user exists
            if (user == null)
                return true;

            // 🔐 Generate secure reset token
            string resetToken = Guid.NewGuid().ToString("N");

            //user.PasswordResetToken = resetToken;
            //user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);

            await _userRepositery.UpdateAsync(user);
            await _userRepositery.CompleteAsync();

            // 📧 TODO: Send email (later)
            // reset link example:
            // https://yourapp.com/Auth/ResetPassword?token={resetToken}

            return true;
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