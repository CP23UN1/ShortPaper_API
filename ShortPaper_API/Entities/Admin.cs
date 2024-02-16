using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Admin
{
    public int AdminId { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string Role { get; set; } = null!;
}
