using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Files
{
    public interface IFileService
    {
        IEnumerable<ShortpaperFile> ListFiles(int shortpaperId);
        ShortpaperFile UploadFile(int shortpaperId, IFormFile file, string fileType, string explanationVideo, string status, string remark, int fileTypeId);

//        Task<Stream> DownloadFile(int fileId);
    }
}
