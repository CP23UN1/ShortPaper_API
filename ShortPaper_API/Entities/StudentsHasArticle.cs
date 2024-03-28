using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class StudentsHasArticle
{
    public string StudentId { get; set; } = null!;

    public int ArticleId { get; set; }

    public virtual Student Student { get; set; } = null!;
}
