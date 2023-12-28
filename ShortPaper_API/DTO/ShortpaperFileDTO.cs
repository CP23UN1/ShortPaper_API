using ShortPaper_API.Entities;

namespace ShortPaper_API.DTO
{
    public class ShortpaperFileDTO
    {
        public int ShortpaperFileId { get; set; }

        public string FileName { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}
