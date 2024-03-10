using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Files
{
    public interface IFileService
    {
        IEnumerable<ShortpaperFile> ListFiles(int shortpaperId);
        ShortpaperFile UploadFile(int shortpaperId, IFormFile file, string explanationVideo, string remark, int fileTypeId);
        byte[] DownloadFile(int fileId);
        ServiceResponse<List<ShortpaperFileTypeDTO>> GetFileType();
        ServiceResponse<List<ShortpaperFileDTO>> GetFiles();
        ServiceResponse<List<ShortpaperFileDTO>> GetFileByStudent(string id);
        ServiceResponse<List<ShortpaperFileDTO>> GetFileByIdAndStudent(int fileId, string studentId);
        ServiceResponse<List<ShortpaperFileDTO>> GetFileByCommittee(int committeeId);

        //        Task<Stream> DownloadFile(int fileId);
    }
}
