using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Files
{
    public interface IFileService
    {
        IEnumerable<ShortpaperFile> ListFiles(int shortpaperId);
        ShortpaperFile UploadFile(int shortpaperId, IFormFile file, string explanationVideo, string remark, int fileTypeId, string studentId);
        byte[] DownloadFile(int shortpaperId, int fileTypeId);
        ServiceResponse<List<ShortpaperFileTypeDTO>> GetFileType();
        ServiceResponse<List<ShortpaperFileDTO>> GetFiles();
        ServiceResponse<List<ShortpaperFileDTO>> GetFileByStudent(string id);
        ServiceResponse<ShortpaperFileDTO> GetFileByIdAndStudent(int fileId, string studentId);
        ServiceResponse<ShortpaperFileDTO> GetFileByTypeIdAndShortpaper(int fileTypeId, int shortpaperId);
        ServiceResponse<List<ShortpaperFileDTO>> GetFileByCommittee(string committeeId);
        public byte[] GeneratePdfPreview(int fileId);
        ServiceResponse<string> UpdateFileStatusToApproved(int fileId);
        ServiceResponse<string> UpdateFileStatusToNotApproved(int fileId);

        //        Task<Stream> DownloadFile(int fileId);
    }
}
