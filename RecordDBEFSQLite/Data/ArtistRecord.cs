using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDBEFSQLite.Data
{
    public class ArtistRecord
    {
        public long ArtistId { get; set; }

        public string? Artist { get; set; }

        public long RecordId { get; set; }

        public string Name { get; set; } = null!;

        public long Recorded { get; set; }

        public string Rating { get; set; } = null!;

        public string Media { get; set; } = null!;
    }
}
