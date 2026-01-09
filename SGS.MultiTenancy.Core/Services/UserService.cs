using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using System.Security.Cryptography;
using System.Text;
namespace SGS.MultiTenancy.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepositery _userRepositery;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserService(IUserRepositery userRepositery, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepositery = userRepositery;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        { 
            User? user = await _userRepositery.FirstOrDefaultAsync(
            x => x.Name.ToLower() == loginRequestDto.UserName.ToLower(),
            query => query.Include(u => u.UserRoles));
           
            bool isPasswordValid = VerifyPassword(loginRequestDto.Password, user.Password);

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

            LoginResponseDto Response = new LoginResponseDto()
            {
                User = userDTO,
                Token = token,
                Roles = userRoles,
                Permissions = userPermissions,
            };

            return Response;
        } 
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
       
        public  bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}