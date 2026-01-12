using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Infra.DataContext;

namespace SGS.MultiTenancy.Infra.Repositery
{
    public class TenantRepository : GenericRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(AppDbContext context) : base(context)
        {
        }
    }
}
