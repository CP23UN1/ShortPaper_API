using Microsoft.EntityFrameworkCore;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using System.Globalization;

namespace ShortPaper_API.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ShortpaperDbContext _db;

        public CommentService(ShortpaperDbContext db)
        {
            _db = db;
        }

        public ServiceResponse<List<CommentDTO>> GetCommentsForFile(int fileId)
        {
            try
            {
                var comments = _db.Comments
               .Where(c => c.FileId == fileId)
               .Select(c => new CommentDTO
               {
                   CommentId = c.CommentId,
                   CommentContent = c.CommentContent,
                   CreatedDatetime = c.CreatedDatetime,
                   FileId = fileId
               })
               .ToList();

                if (comments.Count == 0)
                {
                    var result = new ServiceResponse<List<CommentDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "No comments found."
                    };

                    return result;
                }
                else
                {

                    var result = new ServiceResponse<List<CommentDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status200OK,
                        Data = comments
                    };

                    return result;
                }

            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<List<CommentDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public void AddCommentToFile(AddCommentDTO commentDTO)
        {
//            var file = _db.ShortpaperFiles.Find(commentDTO.FileId, /* Pass other key values here if needed */);
//            if (file == null)
//            {
//                Console.WriteLine($"File with ID {commentDTO.FileId} not found.");
//                return;
//            }

            // Create a new comment entity
            var newComment = new Comment
            {
                CommentContent = commentDTO.CommentContent,
                CreatedDatetime = DateTime.Now,
                UpdatedDatetime = DateTime.Now,
                FileId = commentDTO.FileId
            };

            // Add the comment to the database
            _db.Comments.Add(newComment);
            _db.SaveChanges();

            Console.WriteLine("Comment added successfully.");
        }
    }
}
