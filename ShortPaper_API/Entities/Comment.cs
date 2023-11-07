using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Comment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Comments { get; set; } = null!;

    /// <summary>
    /// 
    /// 
    /// </summary>
    public DateTime CreatedDatetime { get; set; }

    public DateTime UpdatedDatetime { get; set; }

    public virtual User User { get; set; } = null!;
}
