using System;
namespace ShortPaper_API.Services.Files
{
    public class FileStoreService
    {
        private readonly string _basePath;
        private readonly string _uploadsDirectory;

        public FileStoreService(string basePath, string uploadsDirectory)
        {
            _basePath = basePath;
            _uploadsDirectory = uploadsDirectory;

            // Check if the uploads directory exists, if not, create it
            string uploadsPath = Path.Combine(_basePath, _uploadsDirectory);
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
        }

        public void StoreFile(string fileName, Stream fileStream)
        {
            var filePath = Path.Combine(_basePath, _uploadsDirectory, fileName);

            using (var fileWriteStream = File.Create(filePath))
            {
                fileStream.CopyTo(fileWriteStream);
            }
        }

        public Stream GetFile(string fileName)
        {
            var filePath = Path.Combine(_basePath, _uploadsDirectory, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The file does not exist.", filePath);
            }

            return File.OpenRead(filePath);
        }
    }
}

