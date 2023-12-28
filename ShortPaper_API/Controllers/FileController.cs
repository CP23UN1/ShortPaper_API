using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Files;
using ShortPaper_API.DTO;

namespace ShortPaper_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ShortpaperDbContext _dbContext;

        public FileController(IFileService fileService, ShortpaperDbContext dbContext)
        {
            _fileService = fileService;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("list/{shortpaperId}")]
        public IActionResult ListFiles(int shortpaperId)
        {
            try
            {
                var files = _fileService.ListFiles(shortpaperId);
                return Ok(files);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult UploadFile([FromForm] FileUploadDTO model)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the shortpaperId from the model or any other source
            int shortpaperId = model.ShortpaperId;

            // Call the UploadFile method in the FileService
            var uploadedFile = _fileService.UploadFile(
                shortpaperId,
                model.File,
                model.ExplanationVideo,
                model.Remark,
                model.FileTypeId
            );

            // Check if the file upload was successful
            if (uploadedFile != null)
            {
                return Ok(uploadedFile);
            }
            else
            {
                // Handle the case where the file upload failed
                return BadRequest("File upload failed.");
            }
        }

        [HttpGet]
        [Route("download/{fileId}")]
        public IActionResult DownloadFile(int fileId)
        {
            // Call the DownloadFile method from the FileService
            var fileBytes = _fileService.DownloadFile(fileId);

            if (fileBytes == null)
            {
                // File not found
                return NotFound();
            }

            // Retrieve the file information from the database
            var file = _dbContext.ShortpaperFiles.FirstOrDefault(f => f.ShortpaperFileId == fileId);

            // Return the file as a response
            return File(fileBytes, file.FileType, file.FileName);
        }
    }
}
