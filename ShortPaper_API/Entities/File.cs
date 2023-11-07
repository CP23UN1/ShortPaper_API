using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class File
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int StatusId { get; set; }

    public string Filename { get; set; } = null!;

    public string Filesize { get; set; } = null!;

    public string? ExplanationVideo { get; set; }

    public byte[]? Data { get; set; }

    public string? Filetype { get; set; }

    public DateTime CreatedDatetime { get; set; }

    public DateTime UpdatedDatetime { get; set; }

    public virtual FileStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
