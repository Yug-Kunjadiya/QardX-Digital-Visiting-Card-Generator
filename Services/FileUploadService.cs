namespace QardX.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string uploadPath);
        Task<bool> DeleteFileAsync(string filePath);
        bool IsValidImageFile(IFormFile file);
        string GetUniqueFileName(string originalFileName);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileUploadService> _logger;
        private readonly List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png", ".gif", ".svg" };
        private const int MaxFileSize = 5 * 1024 * 1024; // 5MB

        public FileUploadService(IWebHostEnvironment environment, ILogger<FileUploadService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string uploadPath)
        {
            try
            {
                if (!IsValidImageFile(file))
                {
                    throw new ArgumentException("Invalid file type or size");
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, uploadPath);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = GetUniqueFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return Path.Combine(uploadPath, uniqueFileName).Replace('\\', '/');
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file {FileName}", file.FileName);
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FilePath}", filePath);
                return false;
            }
        }

        public bool IsValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.Length > MaxFileSize)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }

        public string GetUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var randomString = Guid.NewGuid().ToString("N")[..8];
            
            return $"{nameWithoutExtension}_{timestamp}_{randomString}{extension}";
        }
    }
}