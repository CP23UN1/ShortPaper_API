using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Files;
using ShortPaper_API.DTO;
using ShortPaper_API.Helper;

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
        [Route("files/{shortpaperId}")]
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
        [Route("file/upload")]
        public IActionResult UploadFile([FromForm] FileUploadDTO model)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int shortpaperId;
            // Get the shortpaperId from the model or any other source
            if (model.ShortpaperId == 0)
            {
                shortpaperId = 0;
            } else
            {
                shortpaperId = model.ShortpaperId;
            }
            

            // Call the UploadFile method in the FileService
            var uploadedFile = _fileService.UploadFile(
                shortpaperId,
                model.File,
                model.ExplanationVideo,
                model.Remark,
                model.FileTypeId,
                model.StudentId
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
        [Route("file/download/{shortpaperId}/{fileTypeId}")]
        public IActionResult DownloadFile(int shortpaperId, int fileTypeId)
        {
            // Call the DownloadFile method from the FileService
            var fileBytes = _fileService.DownloadFile(shortpaperId, fileTypeId);

            if (fileBytes == null)
            {
                // File not found
                return NotFound();
            }

            // Retrieve the file information from the database
            var file = _dbContext.ShortpaperFiles.FirstOrDefault(f => f.ShortpaperId == shortpaperId && f.ShortpaperFileTypeId == fileTypeId );

            // Return the file as a response
            return File(fileBytes, file.FileType, file.FileName);
        }

        [HttpGet]
        [Route("filetype")]
        public ServiceResponse<List<ShortpaperFileTypeDTO>> GetFileType()
        {
            var getFileType = _fileService.GetFileType();
            return getFileType;
        }

        [HttpGet]
        [Route("files")]
        public ServiceResponse<List<ShortpaperFileDTO>> GetFiles()
        {
            var getFiles = _fileService.GetFiles();
            return getFiles;
        }

        [HttpGet]
        [Route("file/search-by-student/{studentId}")]
        public ServiceResponse<List<ShortpaperFileDTO>> GetFileByStudentId(string studentId)
        {
            var getFile = _fileService.GetFileByStudent(studentId);
            return getFile;
        }

        [HttpGet]
        [Route("file/search-by-id-and-student/{fileId}/{studentId}")]
        public ServiceResponse<ShortpaperFileDTO> GetFileByIdAndStudentId(int fileId, string studentId)
        {
            var getFile = _fileService.GetFileByIdAndStudent(fileId, studentId);
            return getFile;
        }

        [HttpGet]
        [Route("file/fileType/Shortpaper/{fileTypeId}/{shortpaperId}")]
        public ServiceResponse<ShortpaperFileDTO> GetFileByTypeAndShortpaper(int fileTypeId, int shortpaperId)
        {
            var getFile = _fileService.GetFileByTypeIdAndShortpaper(fileTypeId, shortpaperId);
            return getFile;
        }

        [HttpGet]
        [Route("file/search-by-committee-id/{committeeId}")]
        public ServiceResponse<List<ShortpaperFileDTO>> GetFileByCommitteeId(string committeeId)
        {
            var getFile = _fileService.GetFileByCommittee(committeeId);
            return getFile;
        }

        [HttpGet]
        [Route("file/preview/{fileId}")]
        public IActionResult GetPreview(int fileId)
        {
            var previewData = _fileService.GeneratePdfPreview(fileId); // Change to GenerateImagePreview if generating image preview
            if (previewData == null)
            {
                return NotFound();
            }

            return File(previewData, "image/jpeg"); // Change the content type as appropriate
        }

    }
}
