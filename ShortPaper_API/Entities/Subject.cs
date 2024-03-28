using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Subject
{
    public string SubjectId { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<StudentsHasSubject> StudentsHasSubjects { get; set; } = new List<StudentsHasSubject>();
}
