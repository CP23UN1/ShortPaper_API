using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class AnnouncementFile
{
    public int AnnouncementFileId { get; set; }

    public string FileName { get; set; } = null!;

    public string FileSize { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public byte[] FileData { get; set; } = null!;

    public DateTime CreatedDatetime { get; set; }

    public int AnnouncementId { get; set; }

    public virtual Announcement Announcement { get; set; } = null!;
}
