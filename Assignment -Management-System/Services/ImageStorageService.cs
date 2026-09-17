using Microsoft.AspNetCore.Http;

namespace Assignment__Management_System.Services
{
    public class ImageStorageService
    {
        public const long MaxImageSize = 5 * 1024 * 1024;

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        private readonly IWebHostEnvironment _environment;

        public ImageStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string SaveImage(IFormFile image, string folderName)
        {
            if (image == null || image.Length == 0)
                throw new InvalidOperationException("Image is required!");

            if (image.Length > MaxImageSize)
                throw new InvalidOperationException("Image cannot exceed 5 MB!");

            var originalFileName = Path.GetFileName(image.FileName);
            var extension = Path.GetExtension(originalFileName);

            if (string.IsNullOrWhiteSpace(originalFileName) || !AllowedExtensions.Contains(extension))
                throw new InvalidOperationException("Only JPG, JPEG, PNG and WEBP images are allowed!");

            var storedFileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
            var uploadDirectory = Path.Combine(_environment.ContentRootPath, "App_Data", folderName);
            var storedFilePath = Path.Combine(uploadDirectory, storedFileName);

            Directory.CreateDirectory(uploadDirectory);

            using var stream = new FileStream(storedFilePath, FileMode.CreateNew);
            image.CopyTo(stream);

            return storedFileName;
        }

        public void DeleteImage(string folderName, string? storedFileName)
        {
            if (string.IsNullOrWhiteSpace(storedFileName))
                return;

            var safeFileName = Path.GetFileName(storedFileName);
            var filePath = Path.Combine(_environment.ContentRootPath, "App_Data", folderName, safeFileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public (byte[] Bytes, string ContentType) ReadImage(string folderName, string storedFileName)
        {
            var safeFileName = Path.GetFileName(storedFileName);
            var filePath = Path.Combine(_environment.ContentRootPath, "App_Data", folderName, safeFileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Image not found on server!");

            return (File.ReadAllBytes(filePath), GetContentType(Path.GetExtension(safeFileName)));
        }

        private static string GetContentType(string extension)
        {
            return extension.ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
