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
        private readonly IGenericRepository<UserRoles> _userRoleRepository;
        public UserService(IUserRepository userRepositery, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasherService passwordHasherService, IGenericRepository<UserRoles> userRoles, IMemoryCache memoryCache, IGenericRepository<UserRoles> userRoleRepository)
        {
            _userRepositery = userRepositery;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasherService = passwordHasherService;
            _userRoles = userRoles;
            _cache = memoryCache;
            _userRoleRepository = userRoleRepository;
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
                TenantID = (Guid)user.TenantID!
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
            HashSet<Guid> permissionIds = await GetUserPermissionIdsAsync(userId, tenantId);
            return permissionIds.Contains(permissionId);
        }

        /// <summary>
        /// Gets all permission IDs for user in tenant.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        private async Task<HashSet<Guid>> GetUserPermissionIdsAsync(Guid userId, Guid tenantId)
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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Return message string</returns>
        public async Task<string> Register(UserDto model)
        {

            var user = await _userRepositery.FirstOrDefaultAsync(
                    u => u.UserName == model.UserName &&
                    u.TenantID == model.TenantId);

            if (user != null)
            {
                return "User already exists.";
            }
            else
            {
                User u = await _userRepositery.AddAsync(new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PasswordHash = _passwordHasherService.HashPassword(model.Password),
                    TenantID = model.TenantId
                });
                await _userRepositery.CompleteAsync();

                await _userRoleRepository.AddAsync(new UserRoles
                {
                    UserID = u!.ID,
                    RoleID = Guid.Parse("93FBE01C-1826-437C-8B9A-58DBF0696DD9"),
                    TenantID = model.TenantId
                });
                await _userRoleRepository.CompleteAsync();
                return "User Registered Sucessfully";
            }
        }

        /// <summary>
        /// Gets users by tenant ID.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns>Returns list of user dto</returns>
        public async Task<List<UserDto>> GetUsersByTenantId(Guid tenantId)
        {

            var query = _userRepositery.Query();

            // Log the generated SQL query for debugging purposes
            var sqlQuery = query.ToQueryString();
            //Console.WriteLine($"Generated SQL Query: {sqlQuery}");
            var users = await _userRepositery.Query()
                .ToListAsync();

            var usersList = users.Select(u => new UserDto
            {
                ID = u.ID,
                UserName = u.UserName,
                Email = u.Email,
                TenantId = u.TenantID
            }).ToList();

            return usersList;
        }

        /// <summary>
        /// Gets user by ID.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>User dto</returns>
        public async Task<UserDto> GetUsersById(Guid Id)
        {
            User user = (User)_userRepositery.Query().Where(u => u.ID == Id).First();
            if (user == null)
                return null;
            return new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                ID = user.ID,
                AvtarUrl = user.AvatarUrl
            };
        }

        /// <summary>
        /// Updates user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usermodel"></param>
        public async Task<UserDto?> UpdateUser(Guid id, UserDto usermodel)
        {
            User user = await _userRepositery.Query()
                .FirstOrDefaultAsync(t => t.ID == usermodel.ID)
                ?? throw new Exception("User not found");

            bool userNameExists = await _userRepositery.AnyAsync(t =>
                t.UserName == usermodel.UserName && t.ID != usermodel.ID);

            if (userNameExists)
                throw new Exception("User Name already exists");

            user.UserName = usermodel.UserName;
            user.Email = usermodel.Email.ToLower();
            user.AvatarUrl = usermodel.AvtarUrl;

            await _userRepositery.UpdateAsync(user);
            await _userRepositery.CompleteAsync();
            return usermodel;
        }

        /// <summary>
        /// Deletes user by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDto?> DeleteUser(Guid id)
        {
            UserDto? user = await GetUsersById(id);

            if (user == null)
            {
                return null;
            }

            User user1 = new User
            {
                ID = id,
                UserName = user.UserName,
                Email = user.Email,
                AvatarUrl = user.AvtarUrl
            };
            await _userRepositery.HardDeleteAsync(id);
            await _userRepositery.CompleteAsync();
            return user;
        }
    }
}