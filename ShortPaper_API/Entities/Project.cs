using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Project
{
    public int ProjectId { get; set; }

    public string? Topic { get; set; }

    public int? CommitteeFirst { get; set; }

    public int? CommitteeSecond { get; set; }

    public int? CommitteeThird { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual User User { get; set; } = null!;
}
