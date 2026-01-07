using SGS.MultiTenancy.Core.Application.Interfaces.Repositories;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;

namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    public interface IUserRepositery : IGenericRepository<User>
    {
    }
}