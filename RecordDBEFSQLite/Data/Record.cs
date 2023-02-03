using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecordDBEFSQLite.Data;

public partial class Record
{
    [Key]
    public int RecordId { get; set; }

    public int ArtistId { get; set; }

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

    public override string ToString()
    {
        return $"Record Id: {RecordId}, Name: {Name}, Recorded: {Recorded}, Media: {Media}";
    }
}
