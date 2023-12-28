using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Files
{
    public class FileService : IFileService
    {
        private readonly ShortpaperDbContext _db;

        public FileService(ShortpaperDbContext db)
        {
            _db = db;
        }

        public IEnumerable<ShortpaperFile> ListFiles(int shortpaperId)
        {
            return _db.ShortpaperFiles
                .Where(file => file.ShortpaperId == shortpaperId)
                .ToList();
        }

        public ShortpaperFile UploadFile(int shortpaperId, IFormFile file, string explanationVideo, string remark, int fileTypeId)
        {
            if (file == null || file.Length == 0)
            {
                // Handle the case where no file is provided
                return null;
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine("wwwroot", "uploads", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Save the file to the specified path
                file.CopyTo(stream);
            }

            // Read file data into a byte array
            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(memoryStream);
                fileData = memoryStream.ToArray();
            }

            var newFile = new ShortpaperFile
            {
                FileName = uniqueFileName,
                FileSize = file.Length.ToString(),
                FileType = file.ContentType,
                FileData = fileData, // Save the raw binary data
                ExplanationVideo = explanationVideo,
                ShortpaperFileTypeId = fileTypeId,
                Remark = remark,
                CreatedDatetime = DateTime.Now,
                UpdatedDatetime = DateTime.Now,
                ShortpaperId = shortpaperId
            };

            _db.ShortpaperFiles.Add(newFile);
            _db.SaveChanges();

            return newFile;
        }

        public byte[] GetFileDataById(int fileId)
        {
            // Assuming you have a method to retrieve file data from the database
            var fileData = _db.ShortpaperFiles
                .Where(f => f.ShortpaperFileId == fileId)
                .Select(f => f.FileData)
                .FirstOrDefault();

            return fileData;
        }

        public byte[] GetFileDataByName(string fileName)
        {
            // Assuming you have a method to retrieve file data from the database
            var fileData = _db.ShortpaperFiles
                .Where(f => f.FileName == fileName)
                .Select(f => f.FileData)
                .FirstOrDefault();

            return fileData;
        }

        public FileResult DownloadFileById(int fileId)
        {
            var fileData = GetFileDataById(fileId);

            if (fileData != null && fileData.Length > 0)
            {
                // Assuming your file has a known content type, replace "application/octet-stream" with the correct content type.
                return new FileContentResult(fileData, "application/octet-stream")
                {
                    FileDownloadName = "your_filename.extension" // Replace with the actual file name and extension
                };
            }

            // If file data is null or empty, you may want to return an appropriate response (e.g., NotFound).
            // For simplicity, returning null in this example.
            return null;
        }

        public FileResult DownloadFileByName(string fileName)
        {
            var fileData = GetFileDataByName(fileName);

            if (fileData != null && fileData.Length > 0)
            {
                // Assuming your file has a known content type, replace "application/octet-stream" with the correct content type.
                return new FileContentResult(fileData, "application/octet-stream")
                {
                    FileDownloadName = fileName // Use the actual file name from the database
                };
            }

            // If file data is null or empty, you may want to return an appropriate response (e.g., NotFound).
            // For simplicity, returning null in this example.
            return null;
        }
    }
}
