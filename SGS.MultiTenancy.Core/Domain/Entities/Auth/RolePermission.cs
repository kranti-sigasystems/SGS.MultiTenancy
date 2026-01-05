using SGS.MultiTenancy.Core.Domain.Common;
using System.Security;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    public class RolePermission : AuditableEntity
    {
        public Guid RoleID { get; set; }
        public Role Role { get; set; }

        public Guid PermissionID { get; set; }
        public Permission Permission { get; set; }

        public Guid TenantID { get; set; }
    }
}
