using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Comments;
using ShortPaper_API.Services.Files;

namespace ShortPaper_API.Controllers
{
    [Route("un1/api")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("/comments")]
        public List<CommentDTO> GetComments(int fileId)
        {
            var comments = _commentService.GetCommentsForFile(fileId);
            return comments;
        }

        [HttpPost]
        [Route("/comments/comment-on-response")]
        public IActionResult CommentOnResponse([FromBody] CommentDTO commentDto)
        {
            if (commentDto == null)
            {
                return BadRequest("Invalid comment data");
            }

            var newComment = _commentService.CommentOnResponse(commentDto.FileId, commentDto.Comments, (int)commentDto.ReplyCommentid);

            // Assuming you want to return the newly created comment
            return Ok(newComment);
        }

    }
}
