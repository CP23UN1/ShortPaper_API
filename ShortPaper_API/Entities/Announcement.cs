using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Announcement
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedDatetime { get; set; }

    public string Status { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public int AuthorId { get; set; }

    public int FileId { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual File File { get; set; } = null!;
}
