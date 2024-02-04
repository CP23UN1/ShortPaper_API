using ShortPaper_API.DTO;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Comments
{
    public interface ICommentService
    {
        ServiceResponse<List<CommentDTO>> GetCommentsForFile(int fileId);
        void AddCommentToFile(AddCommentDTO commentDTO);
    }
}
