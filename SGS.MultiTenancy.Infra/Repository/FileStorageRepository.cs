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

            var uploadRoot = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadRoot);

            var safeName = Path.GetFileName(fileName);
           
            if (string.IsNullOrWhiteSpace(safeName))
                return null;
            var extension = Path.GetExtension(file.FileName);
            var storedName = $"{safeName}{extension}";
            var fullPath = Path.Combine(uploadRoot, storedName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);
            if (stream.Length == 0)
                return null;

            return Path.Combine("uploads", storedName)
                       .Replace("\\", "/");
        }
        public Task<bool> DeleteAsync(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return Task.FromResult(false);

            // Normalize path (avoid ../ attacks)
            var safeRelativePath = relativePath
                .Replace("/", Path.DirectorySeparatorChar.ToString())
                .Replace("\\", Path.DirectorySeparatorChar.ToString());

            var fullPath = Path.Combine(_env.WebRootPath, safeRelativePath);

            if (!File.Exists(fullPath))
                return Task.FromResult(false);

            File.Delete(fullPath);
            return Task.FromResult(true);
        }
    }
}