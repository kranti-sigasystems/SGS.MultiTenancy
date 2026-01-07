namespace SGS.MultiTenancy.Core.Application.DTOs.Auth
{
    public class UserDto
    {
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
