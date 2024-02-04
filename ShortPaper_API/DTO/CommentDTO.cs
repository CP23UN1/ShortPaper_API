namespace ShortPaper_API.DTO
{
    public class CommentDTO
    {
        public int CommentId { get; set; }

        public string CommentContent { get; set; } = null!;

        public DateTime CreatedDatetime { get; set; }

        public DateTime UpdatedDatetime { get; set; }

        public int FileId { get; set; }
    }
}
