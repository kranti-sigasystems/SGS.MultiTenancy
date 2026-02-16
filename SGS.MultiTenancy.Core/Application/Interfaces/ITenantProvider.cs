
namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    public interface ITenantProvider
    {
        //Guid? TenantId { get; }
       
        /// <summary>
        /// Gets or set is hostadmin.
        /// </summary>
        bool IsHostAdmin { get; }

        /// <summary>
        /// Gets or set tenant id.
        /// </summary>
        Guid? TenantId { get; }

        /// <summary>
        /// Gets ot set tenant slug.
        /// </summary>
        string TenantSlug { get; }
    }
}
