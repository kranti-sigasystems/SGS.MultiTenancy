using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SGS.MultiTenancy.Core.Application.Interfaces;

namespace SGS.MultiTenancy.Infra.Repository
{
    public class FileStorageRepository : IFileStorageRepository
    {
        private readonly IWebHostEnvironment _env;

        public FileStorageRepository(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string?> SaveAsync(IFormFile? file, string? fileName)
        {
            if (file == null)
                return null;

            string uploadRoot = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadRoot);

            string safeName = Path.GetFileName(fileName);
           
            if (string.IsNullOrWhiteSpace(safeName))
                return null;
            string extension = Path.GetExtension(file.FileName);
            string storedName = $"{safeName}{extension}";
            string fullPath = Path.Combine(uploadRoot, storedName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);
            if (stream.Length == 0)
                return null;

            return Path.Combine("uploads", storedName)
                       .Replace("\\", "/");
        }
        public bool DeleteAsync(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return false;

            string safeRelativePath = relativePath
                .Replace("/", Path.DirectorySeparatorChar.ToString())
                .Replace("\\", Path.DirectorySeparatorChar.ToString());

            string fullPath = Path.Combine(_env.WebRootPath, safeRelativePath);

            if (!File.Exists(fullPath))
                return false;

            File.Delete(fullPath);
            return true;
        }
    }
}