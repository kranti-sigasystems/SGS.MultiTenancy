using Microsoft.AspNetCore.Http;
using SGS.MultiTenancy.Core.Application.Interfaces;

namespace SGS.MultiTenancy.Infra.Repositery
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid TenantId
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;

                if (context == null || context.User?.Identity?.IsAuthenticated != true)
                    return Guid.Empty; // SuperAdmin / Host / Migrations

                string? tenantClaim =
                    context.User.FindFirst("tenantId")?.Value
                    ?? context.User.FindFirst("tenant_id")?.Value;

                return Guid.TryParse(tenantClaim, out var tenantId)
                    ? tenantId
                    : Guid.Empty;
            }
        }
    }
}
