using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class StudentsHasSubject
{
    public string StudentId { get; set; } = null!;

    public string SubjectId { get; set; } = null!;

    public ulong IsRegisteredSubject { get; set; }

    public ulong IsPaperSubject { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
