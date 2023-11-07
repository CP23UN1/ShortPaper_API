using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Committee
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int? FirstCommitteeId { get; set; }

    public int? SecondCommitteeId { get; set; }

    public int? ThirdCommitteeId { get; set; }

    public virtual User Student { get; set; } = null!;
}
