using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using System.Security.Claims;

namespace SGS.MultiTenancy.UI.Attribute
{
    public sealed class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionAuthorizationHandler(
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            string? userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? tenantId = context.User.FindFirstValue("tenantId");

            if (!Guid.TryParse(userId, out var userGuid) ||
                !Guid.TryParse(tenantId, out var tenantGuid))
                return;

            Guid? permissionId = GetPermissionIdFromPolicy();
            if (permissionId == null)
                return;

            bool allowed = await _userService
                .UserHasPermissionAsync(userGuid, tenantGuid, permissionId.Value);

            if (allowed)
                context.Succeed(requirement);
        }

        private Guid? GetPermissionIdFromPolicy()
        {
            Endpoint? endpoint = _httpContextAccessor.HttpContext?.GetEndpoint();
            IReadOnlyList<IAuthorizeData>? authorizeData = endpoint?
                .Metadata
                .GetOrderedMetadata<IAuthorizeData>();

            string? policy = authorizeData?
                .FirstOrDefault(p => p.Policy?.StartsWith("Permission:") == true)?
                .Policy;

            if (policy == null)
                return null;

            string? raw = policy.Replace("Permission:", "");
            return Guid.TryParse(raw, out var id) ? id : null;
        }
    }
}
