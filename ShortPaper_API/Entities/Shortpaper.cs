using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Shortpaper
{
    public int ShortpaperId { get; set; }

    public string? ShortpaperTopic { get; set; }

    public string StudentId { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
