namespace SGS.MultiTenancy.Core.Application.TenantContext
{
    /// <summary>
    /// Holds tenant information for the current request.
    /// </summary>
    public sealed class TenantContext
    {
        /// <summary>
        /// Gets or sets the resolved tenant identifier.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the tenant slug (subdomain).
        /// </summary>
        public string? TenantSlug { get; set; }

        /// <summary>
        /// Gets or set tje tenant is host.
        /// </summary>
        public bool IsHost { get; set; }
    }

}
