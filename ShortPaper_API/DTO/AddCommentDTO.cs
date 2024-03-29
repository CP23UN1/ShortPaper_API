namespace ShortPaper_API.DTO
{
    public class AddCommentDTO
    {
        public string CommentContent { get; set; } = null!;

        public int FileId { get; set; }

        public int replyId { get; set; }
    }
}
