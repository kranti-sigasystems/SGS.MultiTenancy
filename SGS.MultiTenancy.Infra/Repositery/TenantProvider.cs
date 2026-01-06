using Microsoft.AspNetCore.Http;
using SGS.MultiTenancy.Core.Application.Interfaces;

namespace SGS.MultiTenancy.Infra.Repositery
{
    public class TenantProvider : ITenantProvider
    {
        public Guid TenantId { get; }

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            string? tenantClaim = httpContextAccessor.HttpContext?
                .User?
                .FindFirst("tenant_id")?.Value;

            TenantId = string.IsNullOrWhiteSpace(tenantClaim)
                ? Guid.Empty
                : Guid.Parse(tenantClaim);
        }
    }
}
