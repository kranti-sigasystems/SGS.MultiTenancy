using Microsoft.AspNetCore.Authorization;

namespace SGS.MultiTenancy.UI.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        private const string PolicyPrefix = "Permission:";

        public HasPermissionAttribute(string permissionId)
        {
            Policy = PolicyPrefix + permissionId;
        }
    }
}
