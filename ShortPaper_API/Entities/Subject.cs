using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Subject
{
    public int Id { get; set; }

    public string SubjectId { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<User> UserRegisteredSubjects { get; set; } = new List<User>();

    public virtual ICollection<User> UserShortpaperSubjects { get; set; } = new List<User>();
}
