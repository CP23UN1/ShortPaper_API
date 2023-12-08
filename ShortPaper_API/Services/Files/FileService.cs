using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Files
{
    public class FileService : IFileService
    {
        private readonly ShortpaperDbContext _db;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public FileService(ShortpaperDbContext db, IConfiguration config, ILogger logger)
        {
            _db = db;
            _config = config;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

            // public Entities.File UploadFile(IFormFile file, int projectId, string explanationVideo, int statusId)
            public IActionResult UploadFile(IFormFile file, int projectId, string explanationVideo, int statusId)
        {
            String defaultUploadPath = _config.GetValue<String>("LocalUpload:UploadPath");
            
            try
            {
                if(!Directory.Exists(defaultUploadPath))
                {
                    Directory.CreateDirectory(defaultUploadPath);
                }

                var filePath = Path.Combine(defaultUploadPath, file.FileName);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);

                    var newFile = new Entities.File
                    {
                        Filesize = file.Length.ToString(),
                        Filename = file.FileName,
                        ExplanationVideo = explanationVideo,
                        ProjectId = projectId,
                        StatusId = statusId
                    };
                    _db.Files.Add(newFile);
                    _db.SaveChanges();

                    return new OkObjectResult(newFile);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file: {ErrorMessage}", ex.Message);
                return new StatusCodeResult(500);
            }
        }

        public async Task<(byte[] fileData, string fileName, string contentType)> GetFileDataWithMetadataByIdAsync(int fileId)
        {
            var file = await _db.Files.FindAsync(fileId);

            if (file == null)
            {
                return (null, null, null); // or handle accordingly
            }

            return (file.Data, file.Filename, file.Filetype);
        }

    }
}
