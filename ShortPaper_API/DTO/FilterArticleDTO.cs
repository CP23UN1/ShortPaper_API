namespace ShortPaper_API.DTO
{
    public class FilterArticleDTO
    {
        public string? TopicOrAuthor { get; set; }

        public string? FileName { get; set; }

        public string? Year { get; set; }

        public string? FileType { get; set; }

        public string? Subject { get; set; }
    }
}
