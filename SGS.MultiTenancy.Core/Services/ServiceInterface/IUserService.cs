using SGS.MultiTenancy.Core.Application.DTOs.Auth;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticates a user using the provided login credentials.
        /// </summary>
        /// <param name="loginRequestDto">The login request data containing user credentials.</param>
        /// <returns>Returns login response dto </returns>
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}