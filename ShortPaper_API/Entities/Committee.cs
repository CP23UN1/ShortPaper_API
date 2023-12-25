using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class Committee
{
    public int CommitteeId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? AlternativeEmail { get; set; }

    public string? Password { get; set; }

    public string? Phonenumber { get; set; }

    public ulong IsAdvisor { get; set; }

    public virtual ICollection<ShortpapersHasCommittee> ShortpapersHasCommittees { get; set; } = new List<ShortpapersHasCommittee>();
}
