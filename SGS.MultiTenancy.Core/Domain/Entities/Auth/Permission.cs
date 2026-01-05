using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;


namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    public class Permission : AuditableEntity
    {
        [Key]
        public Guid ID { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}
