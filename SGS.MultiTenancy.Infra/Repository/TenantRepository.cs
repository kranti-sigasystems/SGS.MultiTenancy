using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Infra.DataContext;

namespace SGS.MultiTenancy.Infra.Repository
{
    public class TenantRepository : GenericRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(AppDbContext context) : base(context)
        {
        }
    }
}
