namespace ShortPaper_API.DTO
{
    public class ShortpaperFileForStudentDTO
    {
        public int ShortpaperFileId { get; set; }
        public string FileName { get; set; } = null!;

        public int ShortpaperFileTypeId { get; set; }

        public string Status { get; set; }
        public DateTime UpdatedDatetime { get; set; }
    }
}
