using SGS.MultiTenancy.Core.Domain.Entities.Auth;

namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditLog"></param>
        /// <returns></returns>
       public Task AddAuditLogAsync(AuditLog auditLog, ITenantProvider tenantProvider);
    }
}
