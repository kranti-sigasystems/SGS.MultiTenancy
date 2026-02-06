using Microsoft.AspNetCore.Http;
using SGS.MultiTenancy.Core.Application.Interfaces;

namespace SGS.MultiTenancy.Infra.Repository
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? TenantId
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;

                if (context == null || context.User?.Identity?.IsAuthenticated != true)
                    return null;

                string? tenantClaim =
                    context.User.FindFirst("tenantId")?.Value
                    ?? context.User.FindFirst("tenant_id")?.Value;

                if (Guid.TryParse(tenantClaim, out var tenantId))
                    return tenantId;

                return null;
            }
        }

        public bool IsHostAdmin
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;

                if (context == null)
                    return false;

                var role = context.User.FindFirst("Role")?.Value;

                return role == "SGS_SuperAdmin";
            }
        }
    }
}
