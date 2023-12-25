using ShortPaper_API.Entities;

namespace ShortPaper_API.DTO
{
    public class AnnouncementDTO
    {
        public int AnnouncementId { get; set; }

        public string Topic { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public DateTime? ExpiredDatetime { get; set; }

        //public List<AnnouncementFile>? AnnouncementFiles { get; set; }
    }
}
