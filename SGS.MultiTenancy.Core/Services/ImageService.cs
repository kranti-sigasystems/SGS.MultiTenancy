using Microsoft.AspNetCore.Http;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using Microsoft.AspNetCore.Hosting;
namespace SGS.MultiTenancy.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly string _baseUploadPath;
        public ImageService(string baseUploadPath, IImageService imageService)
        {
            _baseUploadPath = baseUploadPath;
        }

        public async Task<string> SaveAsync(IFormFile file,Guid tenantId)
        {
            string uploadPath = Path.Combine(_baseUploadPath, "uploads");
            Directory.CreateDirectory(uploadPath);

            string extension = Path.GetExtension(file.FileName);

            string fileName = $"{tenantId}{extension}";

            string filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        public void DeleteFile(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            // Remove leading slash
            string cleanPath = relativePath.TrimStart('/');

            if (!File.Exists(cleanPath))
                return;

            try
            {
                File.Delete(cleanPath);
            }
            catch (IOException)
            {
                // optional: log & ignore
            }
        }
    }
}
