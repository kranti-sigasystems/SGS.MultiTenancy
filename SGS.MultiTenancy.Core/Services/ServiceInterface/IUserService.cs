using SGS.MultiTenancy.Core.Application.DTOs.Auth;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    public interface IUserService
    {
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
