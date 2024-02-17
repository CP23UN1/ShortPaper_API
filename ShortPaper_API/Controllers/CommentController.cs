using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using ShortPaper_API.Services.Announcements;
using ShortPaper_API.Services.Comments;

namespace ShortPaper_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ShortpaperDbContext _dbContext;
        private readonly ICommentService _commentService;

        public CommentController(ShortpaperDbContext dbContext, ICommentService commentService)

        {
            _dbContext = dbContext;
            _commentService = commentService;
        }

        [HttpGet]
        [Route("comments/{fileId}")]
        public ServiceResponse<List<CommentDTO>> GetComments(int fileId)
        {
            var comments = _commentService.GetCommentsForFile(fileId);
            return comments;
        }

        [HttpPost]
        [Route("comment/create")]
        public IActionResult AddCommentToFile(AddCommentDTO commentDTO)
        {
            try
            {
                _commentService.AddCommentToFile(commentDTO);
                return Ok("Comment added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
