using ShortPaper_API.DTO;
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Comments
{
    public interface ICommentService
    {
        List<CommentDTO> GetCommentsForFile(int fileId);
        Comment SendResponse(int fileId, string responseText, int? replyCommentId = null);
        Comment CommentOnResponse(int fileId, string commentText, int replyCommentId);
    }
}
