using ShortPaper_API.Entities;
using Microsoft.AspNetCore.Hosting;

namespace ShortPaper_API.Services.Files
{
    public class FileService : IFileService
    {
        private readonly ShortpaperDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileService(ShortpaperDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
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

        public byte[] DownloadFile(int fileId)
    {
        // Retrieve the file from the database
        var file = _db.ShortpaperFiles.FirstOrDefault(f => f.ShortpaperFileId == fileId);

        if (file == null)
        {
            // File not found
            return null;
        }

        // Construct the file path
        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", file.FileName);

        // Read the file into a byte array
        var fileBytes = System.IO.File.ReadAllBytes(filePath);

        return fileBytes;
    }
    }
}
