using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Subject
{
    public string SubjectId { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<Shortpaper> Shortpapers { get; set; } = new List<Shortpaper>();
}
