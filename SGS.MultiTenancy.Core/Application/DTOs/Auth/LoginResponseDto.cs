namespace SGS.MultiTenancy.Core.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
    }
}
