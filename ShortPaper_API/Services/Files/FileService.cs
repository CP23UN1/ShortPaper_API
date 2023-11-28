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
        private readonly IConfiguration _configuration;

        public FileService(ShortpaperDbContext db) { _db = db; }

       // public Entities.File UploadFile(IFormFile file, int projectId, string explanationVideo, int statusId)
        public async Task<Entities.File> UploadFile(IFormFile file, int projectId, string explanationVideo, int statusId)
        {
            if (file == null || file.Length <= 0)
            {
                throw new ArgumentException("File is null or empty");
            }

            // Get file information
            var fileSize = file.Length; // KB
            var fileType = file.ContentType;
            var fileName = Path.GetFileName(file.FileName);

            // Read the file data into a byte array
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var fileData = memoryStream.ToArray();

                // Create a new File entity
                var newFile = new Entities.File
                {
                    Filename = fileName,
                    Filesize = fileSize.ToString(),
                    ExplanationVideo = explanationVideo,
                    Data = fileData,
                    Filetype = fileType,
                    //CreatedDatetime = DateTime.Now,
                    UpdatedDatetime = DateTime.Now,
                    StatusId = statusId,
                    ProjectId = projectId,
                };

                // Add the new file to the database
                _db.Files.Add(newFile);

                // Save changes to the database
                await _db.SaveChangesAsync();

                return newFile;
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
