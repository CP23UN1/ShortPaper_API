using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? StudentId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Role { get; set; }

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Year { get; set; }

    public int RegisteredSubjectid { get; set; }

    public int ShortpaperSubjectid { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual Subject? RegisteredSubject { get; set; } = null!;

    public virtual Subject? ShortpaperSubject { get; set; } = null!;
}
