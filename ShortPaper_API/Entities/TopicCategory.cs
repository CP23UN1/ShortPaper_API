using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class TopicCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<TopicCategoriesHasShortpaper> TopicCategoriesHasShortpapers { get; set; } = new List<TopicCategoriesHasShortpaper>();
}
