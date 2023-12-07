using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortPaper_API.Entities;
using ShortPaper_API.Services;
using ShortPaper_API.Services.Files;
using System.Threading.Tasks;

namespace ShortPaper_API.Controllers
{
    [Route("/api")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Route("/upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, int projectId, string explanationVideo, int statusId)
        {
            try
            {
                var uploadedFile = await _fileService.UploadFile(file, projectId, explanationVideo, statusId);
                return Ok(uploadedFile);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }

        [HttpGet]
        [Route("/download")]
        public async Task<IActionResult> DownloadFile(int fileId)
        {
            try
            {
                var (fileData, fileName, contentType) = await _fileService.GetFileDataWithMetadataByIdAsync(fileId);

                if (fileData == null)
                {
                    return NotFound(); // or handle accordingly
                }

                return File(fileData, contentType, fileName);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your needs
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
