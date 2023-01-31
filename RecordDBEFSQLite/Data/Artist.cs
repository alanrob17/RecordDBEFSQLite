using System;
using System.Collections.Generic;

namespace RecordDBEFSQLite.Data;

public partial class Artist
{
    public long ArtistId { get; set; }

    public string? FirstName { get; set; }

    public string LastName { get; set; } = null!;

    public string? Name { get; set; }

    public string? Biography { get; set; }
}
