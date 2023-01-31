using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDBEFSQLite.Data
{
    public class ArtistData
    {
        public static List<Artist> GetArtists()
        {
            var list = new List<Artist>();
            using (var db = new RecordDbContext())
            {
                return list = db.Artists.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToList();
            }
        }
    }
}
