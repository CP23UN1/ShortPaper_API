using Microsoft.EntityFrameworkCore;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ShortpaperDbContext _db;

        public CommentService(ShortpaperDbContext db)
        {
            _db = db;
        }

        // View comments for a specific file
        public List<CommentDTO> GetCommentsForFile(int fileId)
        {
            return _db.Comments
                .AsNoTracking()
                .Where(c => c.FileId == fileId)
                .Select(c => new CommentDTO
                {
                    Comments = c.Comments,
                    CreatedDatetime = c.CreatedDatetime,
                    UpdatedDatetime = c.UpdatedDatetime,
                    ReplyCommentid = c.ReplyCommentid,
                    FileId = c.FileId
                })
                .ToList();
        }


        // Send a response to a comment
        public Comment SendResponse(int fileId, string responseText, int? replyCommentId = null)
        {
            var newComment = new Comment
            {
                Comments = responseText,
                CreatedDatetime = DateTime.Now,
                UpdatedDatetime = DateTime.Now,
                FileId = fileId,
                ReplyCommentid = replyCommentId
            };

            _db.Comments.Add(newComment);
            _db.SaveChanges();

            return newComment;
        }

        // Comment on a response
        public Comment CommentOnResponse(int fileId, string commentText, int replyCommentId)
        {
            return SendResponse(fileId, commentText, replyCommentId);
        }
    }
}
