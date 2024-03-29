﻿using System;
using System.Collections.Generic;

namespace ShortPaper_API.Entities;

public partial class ShortpapersHasCommittee
{
    public int ShortpaperId { get; set; }

    public string CommitteeId { get; set; } = null!;

    public ulong IsAdvisor { get; set; }

    public ulong IsPrincipal { get; set; }

    public ulong IsCommittee { get; set; }

    public virtual Committee Committee { get; set; } = null!;
}
