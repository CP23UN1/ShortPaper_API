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

        public ShortpaperFile UploadFile(int shortpaperId, IFormFile file, string fileType, string explanationVideo, string status, string remark, int fileTypeId)
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
                FileType = fileType,
                FileData = fileData, // Save the raw binary data
                ExplanationVideo = explanationVideo,
                ShortpaperFileTypeId = fileTypeId,
                Status = status,
                Remark = remark,
                CreatedDatetime = DateTime.Now,
                UpdatedDatetime = DateTime.Now,
                ShortpaperId = shortpaperId
            };

            _db.ShortpaperFiles.Add(newFile);
            _db.SaveChanges();

            return newFile;
        }
    }
}
