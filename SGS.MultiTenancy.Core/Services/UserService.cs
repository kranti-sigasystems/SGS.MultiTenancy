using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using System.Reflection.Metadata;
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
       
        private readonly IUserRepositery _userRepositery;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasherService _passwordHasherService;
        public UserService(IUserRepositery userRepositery, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasherService passwordHasherService)
        {
            _userRepositery = userRepositery;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasherService = passwordHasherService;
        }
        /// <summary>
        /// Validates credentials and returns JWT with roles and permissions.
        /// </summary>
        /// <param name="loginRequestDto">Login request.</param>
        /// <returns>Login response with user info and token.</returns>
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        { 
            User? user = await _userRepositery.FirstOrDefaultAsync(
            x => x.Name.ToLower() == loginRequestDto.UserName.ToLower(),
            query => query.Include(u => u.UserRoles));
           
            if (user == null || !user.UserRoles.Any()
                || !_passwordHasherService.VerifyPassword(loginRequestDto.Password, user.Password))
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

            List<string> userPermissions = roles
                .SelectMany(r => r.RolePermissions)
                .Select(rp => rp.Permission.Code)
                .Distinct()
                .ToList();

            List<string> userRoles = roles
                .Select(r => r.Name)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .ToList();

            string token = _jwtTokenGenerator.GenerateToken(user, userRoles, userPermissions);
            UserDto userDTO = new()
            {
                Email = user.Email,
                ID = user.ID,
                UserName = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto LoginResponse = new LoginResponseDto()
            {
                User = userDTO,
                Token = token,
                Roles = userRoles,
                Permissions = userPermissions,
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
            
            if (!_passwordHasherService.VerifyPassword(currentPassword, user.Password))
            {
                return (false, Constants.CurrentPasswordIncorrect);
            }
            string newPasswordHash = _passwordHasherService.HashPassword(newPassword);
            user.Password = newPasswordHash;

            await _userRepositery.UpdateAsync(user);
            await _userRepositery.CompleteAsync();

            return (true, string.Empty);
        }
    }
}                                             