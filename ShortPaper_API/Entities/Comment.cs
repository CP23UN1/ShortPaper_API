using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Comment
{
    public int CommentId { get; set; }

    public string CommentContent { get; set; } = null!;

    public DateTime CreatedDatetime { get; set; }

    public DateTime UpdatedDatetime { get; set; }

    public int FileId { get; set; }

    public int? ReplyCommentId { get; set; }

    public string AuthorId { get; set; } = null!;
}
