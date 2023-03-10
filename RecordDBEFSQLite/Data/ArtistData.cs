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

        public static string GetArtistName(int artistId)
        {
            var name = string.Empty;

            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);

                if (artist != null)
                {
                   name = artist.Name;
                }
            }

            return name;
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
                else
                {
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
                var artist = context.Artists.FirstOrDefault(a => a.Name.ToLower() == name.ToLower());

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

        /// <summary>
        /// Show an artist as Html.
        /// </summary>
        public static string ShowArtist(int artistId)
        {
            var artistRecord = string.Empty;

            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);

                if (artist != null)
                {
                    artistRecord = ToHtml(artist);
                }
            }

            return artistRecord;
        }

        /// <summary>
        /// ToHtml method shows an instances properties
        /// </summary>
        /// <param name="artist">The artist.</param>
        /// <returns>The <see cref="string"/> artist record as a string.</returns>
        public static string ToHtml(Artist artist)
        {
            var artistDetails = new StringBuilder();

            artistDetails.Append($"<p><strong>ArtistId: </strong>{artist.ArtistId}</p>\n");

            if (!string.IsNullOrEmpty(artist.FirstName))
            {
                artistDetails.Append($"<p><strong>First Name: </strong>{artist.FirstName}</p>\n");
            }

            artistDetails.Append($"<p><strong>Last Name: </strong>{artist.LastName}</p>\n");

            if (!string.IsNullOrEmpty(artist.Name))
            {
                artistDetails.Append($"<p><strong>Name: </strong>{artist.Name}</p>\n");
            }

            if (!string.IsNullOrEmpty(artist.Biography))
            {
                artistDetails.Append($"<p><strong>Biography: </strong></p>\n<div>\n{artist.Biography}\n</div>\n");
            }

            return artistDetails.ToString();
        }

        /// <summary>
        /// Get biography from the current record Id.
        /// </summary>
        public static string GetBiography(int artistId)
        {
            var bio = new StringBuilder();

            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);
                if (artist != null)
                {
                    bio.Append($"Name: {artist.Name}\n");
                    bio.Append($"Biography:\n{artist.Biography}");
                }
            }

            return bio.ToString();
        }

        public static string GetArtistByName(string artistName)
        {
            var artistRecords = new StringBuilder();

            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .Where(r => r.artist.Name != null && r.artist.Name.ToLower().Contains(artistName.ToLower()))
                    .OrderBy(r => r.record.Recorded)
                    .ToList();

                artistRecords.Append($"{artistName}\n");

                if (records.Any())
                {
                    foreach (var r in records)
                    {
                        artistRecords.Append($"\t{r.record.Name} - {r.record.Recorded} - {r.record.Media}\n");
                    }
                }
            }

            return artistRecords.ToString();
        }

        public static string GetArtistById(int artistId)
        {
            var artistRecords = new StringBuilder();

            using (var context = new RecordDbContext())
            {
                var records = context.Records
                                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                                    .Where(r => r.artist.ArtistId == artistId)
                                    .OrderBy(r => r.record.Recorded)
                                    .ToList();

                if (records.Any())
                {
                    artistRecords.Append($"{records[0].artist.ArtistId} - {records[0].artist.Name}\n\n");

                    foreach (var r in records)
                    {
                        artistRecords.Append($"\t{r.record.Name} - {r.record.Recorded} ({r.record.Media})\n");
                    }
                }
            }

            return artistRecords.ToString();
        }

        public static Artist GetArtistEntity(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);
                return artist ?? new Artist { ArtistId = 0 };
            }
        }
    }
}
