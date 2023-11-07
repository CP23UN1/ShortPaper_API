using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class FileStatus
{
    public int StatusId { get; set; }

    public ulong? BOne { get; set; }

    public ulong? PaperOne { get; set; }

    public ulong? PaperTwo { get; set; }

    public ulong? Article { get; set; }

    public ulong? Plagiarism { get; set; }

    public ulong? Copyright { get; set; }

    public ulong? Robbery { get; set; }

    public ulong? Final { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();
}
