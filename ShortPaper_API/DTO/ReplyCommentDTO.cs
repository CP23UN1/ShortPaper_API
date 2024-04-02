namespace ShortPaper_API.DTO
{
    public class ReplyCommentDTO
    {
        public int CommentId { get; set; }

        public string CommentContent { get; set; } = null!;

        public DateTime CreatedDatetime { get; set; }

        public DateTime UpdatedDatetime { get; set; }

        public int? ReplyCommentId { get; set; }

        public string AuthorId { get; set; }
    }
}
