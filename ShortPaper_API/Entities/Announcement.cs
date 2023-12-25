using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Announcement
{
    public int AnnouncementId { get; set; }

    public string Topic { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime CreatedDatetime { get; set; }

    public DateTime? ExpiredDatetime { get; set; }

    public virtual ICollection<AnnouncementFile> AnnouncementFiles { get; set; } = new List<AnnouncementFile>();
}
