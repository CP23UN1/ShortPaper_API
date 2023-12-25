using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class ShortpaperFileType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<ShortpaperFile> ShortpaperFiles { get; set; } = new List<ShortpaperFile>();
}
