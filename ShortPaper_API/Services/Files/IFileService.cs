using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Files
{
    public interface IFileService
    {
        IEnumerable<ShortpaperFile> ListFiles(int shortpaperId);
        ShortpaperFile UploadFile(int shortpaperId, IFormFile file, string explanationVideo, string remark, int fileTypeId);
        byte[] DownloadFile(int fileId);

//        Task<Stream> DownloadFile(int fileId);
    }
}
