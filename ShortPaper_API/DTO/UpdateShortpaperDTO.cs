namespace ShortPaper_API.DTO
{
    public class UpdateShortpaperDTO
    {
        public int ShortpaperId { get; set; }
        public string? ShortpaperTopic { get; set; }
        public string SubjectId { get; set; } = null!;
    }
}
