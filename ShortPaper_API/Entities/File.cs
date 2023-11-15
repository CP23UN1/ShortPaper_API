using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class File
{
    public int Id { get; set; }

    public string Filename { get; set; } = null!;

    public string Filesize { get; set; } = null!;

    public string? ExplanationVideo { get; set; }

    public byte[]? Data { get; set; }

    public string? Filetype { get; set; }

    public DateTime CreatedDatetime { get; set; }

    public DateTime UpdatedDatetime { get; set; }

    public int StatusId { get; set; }

    public int ProjectId { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Project Project { get; set; } = null!;

    public virtual FileStatus Status { get; set; } = null!;
}
