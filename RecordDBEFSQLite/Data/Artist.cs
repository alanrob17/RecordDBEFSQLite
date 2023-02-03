using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecordDBEFSQLite.Data;

public partial class Artist
{
    [Key]
    public int ArtistId { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string LastName { get; set; } = null!;

    [Required]
    public string Name { get; set; }

    public string? Biography { get; set; }

    public override string ToString()
    {
        var biography = string.IsNullOrEmpty(Biography) ? "No Biography" : (Biography.Length > 30 ? Biography.Substring(0, 30) + "..." : Biography);

        return $"Artist Id: {ArtistId}, Artist: {Name}, Biography: {biography}";
    }
}
