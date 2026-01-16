namespace SGS.MultiTenancy.Core.Domain.Common
{
    public static class Utility
    {
        /// <summary>
        /// Prepares the name of the controller for use with URL helpers.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns>The name of the controller for use with URL helpers.</returns>

        public static string? PrepareControllerName(string controllerName)
        {
            return controllerName?.Substring(0, controllerName.LastIndexOf("Controller", StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool CheckUserHasPermission( Guid permissionID)
        {
            return true;
        }
    }
}