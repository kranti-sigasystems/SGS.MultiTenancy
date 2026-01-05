using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    public class Role : AuditableEntity
    {
        [Key]
        public Guid ID { get; set; }


        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
