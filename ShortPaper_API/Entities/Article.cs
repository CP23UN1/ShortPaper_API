using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Article
{
    public int ArticleId { get; set; }

    public string Topic { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? FileName { get; set; }

    public string? FileSize { get; set; }

    public string? FileType { get; set; }

    public string Year { get; set; } = null!;

    public string SubjectId { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
