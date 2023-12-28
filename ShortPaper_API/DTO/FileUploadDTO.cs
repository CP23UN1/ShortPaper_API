namespace ShortPaper_API.DTO
{
    public class FileUploadDTO
    {
        public int ShortpaperId { get; set; }
        public IFormFile File { get; set; }
        public int FileTypeId { get; set; }
        public string? ExplanationVideo { get; set; }
        public string? Remark { get; set; }
    }
}
