using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Application.Interfaces.Repositories;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Entities.Common;
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
        private readonly IAddressRepository _addressRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IFileStorageRepository _fileStorageRepository;
        private readonly ILocationService _locationService;

        /// <summary>
        /// Creates a new user service instance.
        /// </summary>
        /// <param name="userRepositery">User data repository.</param>
        /// <param name="jwtTokenGenerator">JWT generator.</param>
        /// <param name="passwordHasherService">Password hasher.</param>

        public UserService(
            IUserRepository userRepositery,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasherService passwordHasherService,
            IGenericRepository<UserRoles> userRoles,
            IMemoryCache memoryCache,
            IAddressRepository addressRepository,
            IUserAddressRepository userAddressRepository,
            IFileStorageRepository fileStorageRepository,
            ILocationService locationService)
        {
            _userRepositery = userRepositery;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasherService = passwordHasherService;
            _userRoles = userRoles;
            _cache = memoryCache;
            _addressRepository = addressRepository;
            _userAddressRepository = userAddressRepository;
            _fileStorageRepository = fileStorageRepository;
            _locationService = locationService;
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

            if (user == null)
                return true;

            string resetToken = Guid.NewGuid().ToString("N");
            await _userRepositery.UpdateAsync(user);
            await _userRepositery.CompleteAsync();

            return true;
        }

        public async Task<bool> UserHasPermissionAsync(Guid userId, Guid tenantId, Guid permissionId)
        {
            HashSet<Guid> permissionIds = await GetUserPermissionIdsAsync(userId, tenantId);
            return permissionIds.Contains(permissionId);
        }

        /// <summary>
        /// Gets all permission identifiers for user in tenant.
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

            HashSet<Guid> result = permissionIds.ToHashSet();

            _cache.Set(
                cacheKey,
                result,
                TimeSpan.FromMinutes(30));
            return result;
        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="userDto"></param>
        public async Task<UserDto> AddUserAsync(UserDto userDto)
        {
            Guid userId = Guid.NewGuid();

            User? user = _userRepositery.Query(u => u.UserName.ToUpper() == userDto.UserName!.ToUpper() && u.TenantID == userDto.TenantId).FirstOrDefault();
            if (user != null)
            {
                return new UserDto
                {
                    UserName = user.UserName,
                    TenantId = userDto.TenantId,
                };
            }
            userDto.AvtarUrl = await _fileStorageRepository.SaveAsync(userDto.ProfileImage, userId.ToString());

            user = await _userRepositery.AddAsync(new User()
            {
                ID = userId,
                Email = userDto.Email,
                UserName = userDto.UserName,
                TenantID = userDto.TenantId,
                PasswordHash = _passwordHasherService.HashPassword(userDto.Password),
                AvatarUrl = userDto.AvtarUrl,
                CreateBy = (Guid)userDto.TenantId!,
                CreateOn = DateTime.UtcNow,
            });
            await _userRepositery.CompleteAsync();

            if (userDto.Addresses != null && userDto.Addresses.Any())
            {
                foreach (CreateUserAddressDto addressDto in userDto.Addresses)
                {
                    Address address = new Address
                    {
                        ID = Guid.NewGuid(),
                        PhoneNumber = addressDto.PhoneNumber,
                        AddressLine = addressDto.AddressLine,
                        PostalCode = addressDto.PostalCode,
                        City = addressDto.City,
                        State = addressDto.State,
                        Country = addressDto.Country,
                        TenantID = userDto.TenantId,
                        IsDefault = addressDto.IsDefault
                    };

                    await _addressRepository.AddAsync(address);
                    await _addressRepository.CompleteAsync();

                    await _userAddressRepository.AddAsync(new UserAddress
                    {
                        UserID = user.ID,
                        AddressId = address.ID,
                        TenantID = userDto.TenantId
                    });
                    await _userAddressRepository.CompleteAsync();
                }
            }

            if (userDto.RoleIds != null && userDto.RoleIds.Any())
            {
                foreach (Guid roleId in userDto.RoleIds)
                {
                    await _userRoles.AddAsync(new UserRoles
                    {
                        UserID = user.ID,
                        RoleID = roleId,
                        TenantID = userDto.TenantId
                    });
                    await _userRoles.CompleteAsync();
                }
            }
            userDto.ID = user.ID;
            return userDto;
        }
        /// <summary>
        /// Fetch users related to specific tenant.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public Task<List<UserDto>> GetUsersByTenantAsync(Guid tenantId)
        {
            return _userRepositery
                .Query(u => u.TenantID == tenantId && u.Status == EntityStatus.Active)
                .AsNoTracking()
                .OrderBy(u => u.UserName)
                .Select(u => new UserDto
                {
                    ID = u.ID,
                    UserName = u.UserName,
                    Email = u.Email,
                    AvtarUrl = u.AvatarUrl,
                    TenantId = u.TenantID,
                    Status = u.Status,
                    Addresses = u.UserAddresses
                        .Select(ua => new CreateUserAddressDto
                        {
                            PhoneNumber = ua.Address.PhoneNumber,
                            AddressLine = ua.Address.AddressLine,
                            PostalCode = ua.Address.PostalCode,
                            City = ua.Address.City,
                            State = ua.Address.State,
                            Country = ua.Address.Country,
                            IsDefault = ua.Address.IsDefault
                        }).ToList()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="userDto"></param>

        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            User? user = await _userRepositery
                .Query(u => u.UserName.ToUpper() == userDto.UserName!.ToUpper()
                            && u.TenantID == userDto.TenantId)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return userDto;
            }
            user.Email = userDto.Email;
            user.UserName = userDto.UserName;
            if (userDto.ProfileImage is not null)
            {
                if (!string.IsNullOrWhiteSpace(user.AvatarUrl))
                {
                    _fileStorageRepository.DeleteAsync(user.AvatarUrl);
                }
                user.AvatarUrl = await _fileStorageRepository.SaveAsync(userDto.ProfileImage, user.ID.ToString());
            }

            if (userDto.Addresses != null && userDto.Addresses.Any())
            {
                List<UserAddress>? userAddresses = await _userAddressRepository.Query(ua => ua.UserID == user.ID).ToListAsync();
                if (!userAddresses.Any())
                {

                    List<Task<Address>> newAddresses = userDto.Addresses.Select(async a => new Address
                    {
                        ID = Guid.NewGuid(),
                        PhoneNumber = a.PhoneNumber,
                        AddressLine = a.AddressLine,
                        PostalCode = a.PostalCode,
                        City = a.City,
                        State = await _locationService.GetStateNameByIdAsync(a.State),
                        Country = await _locationService.GetCountryNameByIdAsync(a.Country),
                        TenantID = userDto.TenantId,
                        IsDefault = a.IsDefault
                    }).ToList();

                    Address[]? addresses = await Task.WhenAll(newAddresses);
                    await _addressRepository.AddRangeAsync(addresses);
                    await _addressRepository.CompleteAsync();

                    List<UserAddress>? userAddressRelations = addresses.Select(addr => new UserAddress
                    {
                        TenantID = userDto.TenantId,
                        UserID = user.ID,
                        AddressId = addr.ID
                    }).ToList();

                    await _userAddressRepository.AddRangeAsync(userAddressRelations);
                    await _userAddressRepository.CompleteAsync();
                }
                else
                {
                    foreach (var userAddress in userAddresses)
                    {
                        Address? address = await _addressRepository.Query(a => a.ID == userAddress.AddressId).FirstOrDefaultAsync();

                        if (address != null)
                        {
                            CreateUserAddressDto? dtoAddress = userDto.Addresses.FirstOrDefault();

                            if (dtoAddress != null)
                            {
                                if (!string.IsNullOrWhiteSpace(dtoAddress.PhoneNumber))
                                    address.PhoneNumber = dtoAddress.PhoneNumber;

                                if (!string.IsNullOrWhiteSpace(dtoAddress.AddressLine))
                                    address.AddressLine = dtoAddress.AddressLine;

                                if (!string.IsNullOrWhiteSpace(dtoAddress.City))
                                    address.City = dtoAddress.City;

                                if (!string.IsNullOrWhiteSpace(dtoAddress.State))
                                    address.State = await _locationService.GetStateNameByIdAsync(dtoAddress.State);

                                if (!string.IsNullOrWhiteSpace(dtoAddress.PostalCode))
                                    address.PostalCode = dtoAddress.PostalCode;

                                if (!string.IsNullOrWhiteSpace(dtoAddress.Country))
                                    address.Country = await _locationService.GetCountryNameByIdAsync(dtoAddress.Country);

                                await _addressRepository.UpdateAsync(address);
                                await _addressRepository.CompleteAsync();
                            }
                        }
                    }
                }
            }
            await _userRepositery.UpdateAsync(user);
            return userDto;
        }

        /// <summary>
        /// Marks the specified user as deleted within the given tenant context.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        public async Task<bool> DeleteUserAsync(Guid userId, Guid tenantId)
        {

            User? user = _userRepositery.Query(u => u.ID == userId && u.TenantID == tenantId).FirstOrDefault();

            if (user == null)
            {
                return false;
            }
            user.Status = EntityStatus.Deleted;
            user.LastUpdateOn = DateTime.UtcNow;
            user.LastUpdateBy = tenantId;
            await _userRepositery.UpdateAsync(user);
            await _userRepositery.CompleteAsync();
            return true;
        }

        /// <summary>
        /// Gets user details by user ID and tenant ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        /// <returns>UserDto</returns>
        public async Task<UserDto> GetUserByIdAsync(Guid userId, Guid tenantId)
        {
            User? user = await _userRepositery
                .Query(u => u.ID == userId && u.TenantID == tenantId && u.Status == EntityStatus.Active)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            List<UserAddress> addresses = await _userAddressRepository.Query(ua => ua.UserID == user.ID)
                  .Include(ua => ua.Address).ToListAsync();

            List<CreateUserAddressDto>? addressList = addresses?
            .Select(address => new CreateUserAddressDto
               {
                   Id = address.Address.ID,
                   PhoneNumber = address.Address.PhoneNumber,
                   AddressLine = address.Address.AddressLine,
                   PostalCode = address.Address.PostalCode,
                   City = address.Address.City,
                   State = address.Address.State,
                   Country = address.Address.Country,
                   IsDefault = address.Address.IsDefault
               })
               .ToList();
            UserDto userDto = new UserDto
            {
                ID = user.ID,
                UserName = user.UserName,
                Email = user.Email, 
                AvtarUrl = user.AvatarUrl,
                TenantId = user.TenantID,
                Status = user.Status,
                Addresses = addressList
            };
            return userDto;
        }
    }
}