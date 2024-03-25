using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Student
{
    public string StudentId { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string? AlternativeEmail { get; set; }

    public string? Phonenumber { get; set; }

    public string? Status { get; set; }

    public string Year { get; set; } = null!;

    public virtual ICollection<Shortpaper> Shortpapers { get; set; } = new List<Shortpaper>();

    public virtual ICollection<StudentsHasSubject> StudentsHasSubjects { get; set; } = new List<StudentsHasSubject>();
}
