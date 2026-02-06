using Microsoft.AspNetCore.Http;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    public interface IImageService
    {
        Task<string> SaveAsync(IFormFile file,Guid tenantId);
    }
}