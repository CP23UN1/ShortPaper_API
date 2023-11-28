namespace ShortPaper_API.Services.Files
{
    public interface IFileService
    {
        Task<Entities.File> UploadFile(IFormFile file, int projectId, string explanationVideo, int statusId);

        Task<(byte[] fileData, string fileName, string contentType)> GetFileDataWithMetadataByIdAsync(int fileId);
    }

}
