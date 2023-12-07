using ShortPaper_API.Entities;

namespace ShortPaper_API.DTO
{
    public class AnnouncementDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public string Status { get; set; }

        public string? ImageUrl { get; set; }

        public int AuthorId { get; set; }

        public int FileId { get; set; }
    }
}
