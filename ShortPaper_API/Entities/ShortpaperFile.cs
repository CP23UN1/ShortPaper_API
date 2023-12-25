using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class ShortpaperFile
{
    public int ShortpaperFileId { get; set; }

    public string FileName { get; set; } = null!;

    public string FileSize { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string FileLink { get; set; } = null!;

    public string? ExplanationVideo { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? UpdatedStatusDatetime { get; set; }

    public string? Remark { get; set; }

    public DateTime CreatedDatetime { get; set; }

    public DateTime UpdatedDatetime { get; set; }

    public int ShortpaperId { get; set; }

    public int ShortpaperFileTypeId { get; set; }

    public virtual ShortpaperFileType ShortpaperFileType { get; set; } = null!;
}
