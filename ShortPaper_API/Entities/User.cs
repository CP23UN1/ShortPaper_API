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

    public string? ProjectTopic { get; set; }

    public string? RegisteredSubjectId { get; set; }

    public string? ProjectSubjectId { get; set; }

    public string? Year { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Committee> Committees { get; set; } = new List<Committee>();

    public virtual ICollection<File> Files { get; set; } = new List<File>();
}
