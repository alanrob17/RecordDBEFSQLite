﻿using System;
using System.Collections.Generic;

namespace RecordDBEFSQLite.Data;

public partial class Record
{
    public long RecordId { get; set; }

    public long ArtistId { get; set; }

    public string Name { get; set; } = null!;

    public string Field { get; set; } = null!;

    public long Recorded { get; set; }

    public string Label { get; set; } = null!;

    public string Pressing { get; set; } = null!;

    public string Rating { get; set; } = null!;

    public long Discs { get; set; }

    public string Media { get; set; } = null!;

    public string? Bought { get; set; }

    public double? Cost { get; set; }

    public string? Review { get; set; }
}