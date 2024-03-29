﻿using ShortPaper_API.DTO;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Comments
{
    public interface ICommentService
    {
        ServiceResponse<List<CommentDTO>> GetCommentsForFile(int fileId);
        ServiceResponse<List<CommentDTO>> GetCommentsFormReplyId(int fileId, int replyId);
        ServiceResponse<AddCommentDTO> AddCommentToFile(AddCommentDTO commentDTO);
        ServiceResponse<AddCommentDTO> AddReplyComment(AddCommentDTO commentDTO);

    }
}
