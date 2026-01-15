using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Infra.DataContext;
using SGS.MultiTenancy.Infra.Repository;

namespace SGS.MultiTenancy.Infra.Repositery
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}