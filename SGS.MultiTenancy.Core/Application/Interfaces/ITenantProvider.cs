
namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    public interface ITenantProvider
    {
     /// <summary>
     /// Gets or sets tenant id.
     /// </summary>
        Guid? TenantId { get; }

        /// <summary>
        /// Gets or sets a IsHostAdmin.
        /// </summary>
        bool IsHostAdmin { get; }
    }
}