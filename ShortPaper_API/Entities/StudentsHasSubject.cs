using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class StudentsHasSubject
{
    public string StudentsStudentId { get; set; } = null!;

    public string SubjectsSubjectId { get; set; } = null!;

    public ulong IsRegisteredSubject { get; set; }

    public ulong IsPaperSubject { get; set; }

    public virtual Student StudentsStudent { get; set; } = null!;

    public virtual Subject SubjectsSubject { get; set; } = null!;
}
