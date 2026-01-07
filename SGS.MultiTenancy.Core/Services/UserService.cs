using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;

namespace SGS.MultiTenancy.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepositery _userRepositery;
        public UserService(IUserRepositery userRepositery)
        {
            this._userRepositery = userRepositery;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                User? user = _userRepositery.GetAllIncluding(x => x.UserRoles).FirstOrDefault(x => x.Name.ToLower() == loginRequestDto.UserName.ToLower());
                
                if (user == null || !user.UserRoles.Any())
                {
                    return new LoginResponseDto()
                    {
                        User = null,
                        Token = " ",
                    };
                }


            }
            catch (Exception ex)
            {

                throw;
            }
           

            // if user was found , Generate JWT Token

            //var roles = await _userManager.GetRolesAsync(user);
            //var token = _jwtTokenGenerator.GenerateToken(user, roles);

            //UserDto userDTO = new()
            //{
            //    Email = user.Email,
            //    ID = user.Id,
            //    Name = user.Name,
            //    PhoneNumber = user.PhoneNumber
            //};

            LoginResponseDto loginResponseDto = new LoginResponseDto();
            

            return loginResponseDto;
        }
    }
}
