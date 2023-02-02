using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

        /// <summary>
        /// Check if an Artist already exists.
        /// </summary>
        public static bool CheckForArtistName(string name)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.Where(a => a.Name == name).FirstOrDefault();
                if (artist is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Insert a new artist entity.
        /// </summary>
        public static int InsertArtist(Artist artist)
        {
            using (var context = new RecordDbContext())
            {
                context.Artists.Add(artist);
                context.SaveChanges();

                var newArtist = context.Artists.OrderByDescending(x => x.ArtistId).FirstOrDefault();

                if (newArtist != null)
                {
                    return newArtist.ArtistId;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Update an artist.
        /// </summary>
        public static bool UpdateArtist(Artist artist)
        {
            using (var context = new RecordDbContext())
            {
                var artistToUpdate = context.Artists.FirstOrDefault(a => a.ArtistId == artist.ArtistId);

                if (artistToUpdate != null)
                {
                    artistToUpdate.FirstName = artist.FirstName;
                    artistToUpdate.LastName = artist.LastName;
                    artistToUpdate.Name = string.IsNullOrEmpty(artist.FirstName) ? artist.LastName : $"{artist.FirstName} {artist.LastName}";
                    artistToUpdate.Biography = artist.Biography;

                    context.SaveChanges();

                    return true;
                }
                else { 
                    return false; 
                }
            }
        }

        /// <summary>
        /// Delete an artist.
        /// </summary>
        public static bool DeleteArtist(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(r => r.ArtistId == artistId);

                if (artist != null)
                {
                    context.Artists.Remove(artist);
                    context.SaveChanges();

                    return true;
                }
                else 
                { 
                    return false;
                }
            }
        }

        /// <summary>
        /// Get artist id by firstName, lastName.
        /// </summary>
        public static int GetArtistId(string firstName, string lastName)
        {
            var name = string.IsNullOrEmpty(firstName) ? lastName : $"{firstName} {lastName}";

            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.Name == name);

                if (artist != null)
                {
                    return artist.ArtistId;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
