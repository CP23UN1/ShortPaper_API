namespace ShortPaper_API.DTO
{
    public class AddShortpaperDTO
    {
        public string? ShortpaperTopic { get; set; }

        public string StudentId { get; set; } = null!;

        public string SubjectId { get; set; } = null!;
    }
}
