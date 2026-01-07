using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Infra.DataContext;
using SGS.MultiTenancy.Infra.Repository;

namespace SGS.MultiTenancy.Infra.Repositery
{
    public class UserRepositery : GenericRepository<User>, IUserRepositery
    {
        public UserRepositery(AppDbContext appDbContext) : base(appDbContext)
        {
                
        }
    }
}
