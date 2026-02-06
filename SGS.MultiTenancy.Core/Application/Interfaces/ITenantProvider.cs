
namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    public interface ITenantProvider
    {
        Guid? TenantId { get; }
        bool IsHostAdmin { get; }
    }
}
