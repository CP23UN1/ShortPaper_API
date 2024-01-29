using ShortPaper_API.Entities;

namespace ShortPaper_API.DTO
{
    public class ShortpaperFileDTO
    {
        public int ShortpaperFileId { get; set; }
        public string FileName { get; set; } = null!;

        //Jajah add to make getFiles
        public ShortpaperFileTypeDTO? ShortpaperFileType { get; set; }
        public DateTime UpdatedDatetime { get; set; }
    }
}
