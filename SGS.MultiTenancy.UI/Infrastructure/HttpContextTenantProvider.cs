using SGS.MultiTenancy.Core.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace SGS.MultiTenancy.UI.Infrastructure
{
    public class HttpContextTenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _accessor;

        public HttpContextTenantProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid TenantId
        {
            get
            {
                var context = _accessor.HttpContext;
                if (context == null) return Guid.Empty;

                // Try Items, headers, or claims
                if (context.Items.ContainsKey("TenantId") && context.Items["TenantId"] is Guid tGuid)
                    return tGuid;

                if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var headerValue) && Guid.TryParse(headerValue.FirstOrDefault(), out var hGuid))
                    return hGuid;

                var claim = context.User?.FindFirst("tenantid") ?? context.User?.FindFirst("tenant_id") ?? context.User?.FindFirst("TenantId") ?? context.User?.FindFirst("tid");
                if (claim != null && Guid.TryParse(claim.Value, out var cTid))
                    return cTid;

                return Guid.Empty;
            }
        }
    }
}
