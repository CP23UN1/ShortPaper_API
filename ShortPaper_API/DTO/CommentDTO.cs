namespace ShortPaper_API.DTO
{
    public class CommentDTO
    {
        public string Comments { get; set; } = null!;

        /// <summary>
        /// 
        /// 
        /// </summary>
        public DateTime CreatedDatetime { get; set; }

        public DateTime UpdatedDatetime { get; set; }

        public int? ReplyCommentid { get; set; }

        public int FileId { get; set; }
    }
}
