using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Comment
{
    public int Id { get; set; }

    public string Comments { get; set; } = null!;

    /// <summary>
    /// 
    /// 
    /// </summary>
    public DateTime CreatedDatetime { get; set; }

    public DateTime UpdatedDatetime { get; set; }

    public int? ReplyCommentid { get; set; }

    public int FileId { get; set; }

    public virtual File File { get; set; } = null!;

    public virtual ICollection<Comment> InverseReplyComment { get; set; } = new List<Comment>();

    public virtual Comment? ReplyComment { get; set; }
}
