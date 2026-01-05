using SGS.MultiTenancy.Core.Domain.Common;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    public class User : AuditableEntity
    {
        public Guid ID { get; set; }

        public Guid TenantID { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsMobileConfirmed { get; set; }
        public bool IsEmailConfirmed { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; }
    }
}