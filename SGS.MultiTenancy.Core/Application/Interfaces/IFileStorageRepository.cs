using Microsoft.AspNetCore.Http;

namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    public interface IFileStorageRepository
    {
        /// <summary>
        /// Save a images.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<string?> SaveAsync(IFormFile? file, string? fileName);

        /// <summary>
        /// Delete a image by relative path.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string? relativePath);
    }

}