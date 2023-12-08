using Microsoft.AspNetCore.Mvc;

namespace ShortPaper_API.Services.Files
{
    public interface IFileService
    {
        IActionResult UploadFile(IFormFile file, int projectId, string explanationVideo, int statusId);

        Task<(byte[] fileData, string fileName, string contentType)> GetFileDataWithMetadataByIdAsync(int fileId);
    }

}
