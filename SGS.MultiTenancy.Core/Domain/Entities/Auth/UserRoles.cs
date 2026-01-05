using SGS.MultiTenancy.Core.Domain.Common;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    public class UserRoles : AuditableEntity
    {
        public Guid UserID { get; set; }
        public User User { get; set; }

        public Guid RoleID { get; set; }
        public Role Role { get; set; }

        public Guid TenantID { get; set; }
    }
}