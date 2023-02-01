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

        public static Artist GetArtistName(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);

                if (artist != null)
                {
                    return artist;
                }
                else
                {
                    artist = new()
                    {
                        ArtistId = 0
                    };

                    return artist;
                }
            }
        }

        public static Artist GetArtist(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);

                if (artist != null)
                {
                    return artist;
                }
                else
                {
                    artist = new()
                    {
                        ArtistId = 0
                    };

                    return artist;
                }
            }
        }

        public static List<Artist> GetArtistsWithNoBio()
        {
            using (var context = new RecordDbContext())
            {
                var artists = context.Artists.Where(a => string.IsNullOrEmpty(a.Biography))
                    .OrderBy(a => a.LastName)
                    .ThenBy(a => a.FirstName)
                    .ToList();

                return artists;
            }
        }

        public static int NoBiographyCount()
        {
            var number = 0;
            using (var context = new RecordDbContext())
            {
                number = context.Artists.Where(a => string.IsNullOrEmpty(a.Biography)).Count();
            }
                
            return number;
        }
    }
}
