using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class TopicCategoriesHasShortpaper
{
    public int CategoryId { get; set; }

    public int ShortpaperId { get; set; }

    public virtual TopicCategory Category { get; set; } = null!;
}
