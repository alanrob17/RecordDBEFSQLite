using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RecordDBEFSQLite.Data
{
    public class RecordData
    {
        public static List<Record> GetRecords()
        {
            var list = new List<Record>();
            using (var context = new RecordDbContext())
            {
                return list = context.Records.OrderBy(r => r.ArtistId).ThenBy(r => r.Recorded).ToList();
            }
        }

        /// <summary>
        /// Get Artist and their record details.
        /// Called by GetRecordList3
        /// </summary>
        public static IEnumerable<dynamic> GetRecordList()
        {
            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .OrderBy(r => r.artist.LastName)
                    .ThenBy(r => r.artist.FirstName)
                    .ThenBy(r => r.record.Recorded)
                    .ToList();

                return records.Select(r => new
                {
                    ArtistId = r.artist.ArtistId,
                    Artist = r.artist.Name,
                    RecordId = r.record.RecordId,
                    Name = r.record.Name,
                    Recorded = r.record.Recorded,
                    Rating = r.record.Rating,
                    Media = r.record.Media
                });
            }
        }

        public static IEnumerable<dynamic> GetArtistRecordByYear(int year)
        {
            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .Where(r => r.record.Recorded == year)
                    .OrderBy(r => r.artist.LastName)
                    .ThenBy(r => r.artist.FirstName)
                    .ToList();

                return records.Select(r => new
                {
                    ArtistId = r.artist.ArtistId,
                    Artist = r.artist.Name,
                    RecordId = r.record.RecordId,
                    Name = r.record.Name,
                    Recorded = r.record.Recorded,
                    Rating = r.record.Rating,
                    Media = r.record.Media
                });
            }
        }

        public static dynamic? GetArtistRecordEntity(int recordId)
        {
            using (var context = new RecordDbContext())
            {
                var record = context.Records
                    .Join(context.Artists, r => r.ArtistId, a => a.ArtistId, (r, a) => new { record = r, artist = a })
                    .Where(r => r.record.RecordId == recordId)
                    .Select(r => new
                    {
                        ArtistId = r.artist.ArtistId,
                        Artist = r.artist.Name,
                        RecordId = r.record.RecordId,
                        Name = r.record.Name,
                        Recorded = r.record.Recorded,
                        Rating = r.record.Rating,
                        Media = r.record.Media
                    })
                    .FirstOrDefault();

                if (record != null)
                {
                    return record;
                }
            }

            return null;
        }

        /// <summary>
        /// Get record details including the artist name.
        /// TODO: Change this to an anonymous method - remove ArtistRecord class.
        /// </summary>
        public static string GetArtistRecord(int recordId)
        {
            var artistRecord = new StringBuilder();

            using (var context = new RecordDbContext())
            {
                var record = context.Records.FirstOrDefault(r => r.RecordId == recordId);

                if (record is Record)
                {
                    var artist = context.Artists.FirstOrDefault(r => r.ArtistId == record.ArtistId);

                    if (artist is Artist)
                    {
                        artistRecord.Append($"{artist.Name}");
                    }

                    artistRecord.Append($" {record.ToString()}");
                }
                else
                {
                    artistRecord.Append($"Record with Id: {recordId} not found!");
                }
            }

            return artistRecord.ToString();
        }

        /// <summary>
        /// Get record details.
        /// </summary>
        public static Record GetRecordEntity(int recordId)
        {
            using (var context = new RecordDbContext())
            {
                var record = context.Records.FirstOrDefault(r => r.RecordId == recordId);

                if (record is Record)
                {
                    return record;
                }
                else
                {
                    Record missingRecord = new()
                    {
                        RecordId = 0
                    };

                    return missingRecord;
                }
            }
        }

        /// <summary>
        /// Count the number of discs.
        /// </summary>
        public static int CountAllDiscs(string media = "")
        {
            StringBuilder count = new StringBuilder();
            var mediaTypes = new List<string>();

            switch (media)
            {
                case "":
                    mediaTypes = new List<string> { "DVD", "CD/DVD", "Blu-ray", "CD/Blu-ray", "CD", "R" };
                    break;
                case "DVD":
                    mediaTypes = new List<string> { "DVD", "CD/DVD", "Blu-ray", "CD/Blu-ray" };
                    break;
                case "CD":
                    mediaTypes = new List<string> { "CD" };
                    break;
                case "R":
                    mediaTypes = new List<string> { "R" };
                    break;
                default:
                    break;
            }

            using (var context = new RecordDbContext())
            {
                var sumOfDiscs = context.Records
                    .Where(r => mediaTypes.Contains(r.Media))
                    .Sum(r => r.Discs);

                return (int)sumOfDiscs;
            }
        }

        /// <summary>
        /// Get number of records for an artist.
        /// </summary>
        public static int GetArtistNumberOfRecords(int artistId)
        {
            var discs = 0;

            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);

                var sumOfDiscs = context.Records
                    .Where(r => r.ArtistId == 114)
                    .Select(r => r.Discs)
                    .Sum();

                if (sumOfDiscs > 0 && artist is Artist)
                {
                    discs = (int)sumOfDiscs;
                }
            }

            return discs;
        }

        /// <summary>
        /// Get record details from ToString method.
        /// </summary>
        public static string GetFormattedRecord(int recordId)
        {
            var recordDetails = string.Empty;

            using (var context = new RecordDbContext())
            {
                var record = context.Records.SingleOrDefault(r => r.RecordId == recordId);

                if (record is Record)
                {
                    recordDetails = record.ToString();
                }
            }

            return recordDetails;
        }

        public static string GetArtistNameFromRecord(int recordId)
        {
            var artistName = string.Empty;

            using (var context = new RecordDbContext())
            {
                var record = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .Where(r => r.record.RecordId == recordId)
                    .ToList();

                if (record.Any())
                {
                    artistName = record[0].artist.Name;
                }
            }

            return artistName;
        }

        /// <summary>
        /// Get the number of discs for a particular year.
        /// </summary>
        public static int GetDiscCountForYear(int year)
        {
            var discCount = 0;

            using (var context = new RecordDbContext())
            {
                var records = context.Records.ToList();

                var numberOfDiscs = records
                    .Where(r => r.Recorded == year)
                    .Select(r => r.Discs)
                    .Sum();

                discCount = (int)numberOfDiscs;
            }

            return discCount;
        }

        /// <summary>
        /// Get the number of discs that I bought for a particular year.
        /// </summary>
        public static int GetBoughtDiscCountForYear(int year)
        {
            var discCount = 0;

            using (var context = new RecordDbContext())
            {
                var records = context.Records.ToList();

                var numberOfDiscs = records
                    .Where(r => !string.IsNullOrEmpty(r.Bought) && r.Bought.Contains(year.ToString()))
                    .Select(r => r.Discs)
                    .Sum();

                discCount = (int)numberOfDiscs;
            }

            return discCount;
        }

        /// <summary>
        /// Get a list of records without reviews.
        /// TODO: Change this to an anonymous method - remove ArtistRecord class.
        /// </summary>
        public static List<ArtistRecord> MissingRecordReviews()
        {
            List<ArtistRecord> artistRecords = new();

            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { artist, record })
                    .Where(r => string.IsNullOrEmpty(r.record.Review))
                    .OrderBy(r => r.artist.LastName).ThenBy(r => r.artist.FirstName)
                    .ToList();

                foreach (var r in records)
                {
                    ArtistRecord ar = new()
                    {
                        ArtistId = r.artist.ArtistId,
                        Artist = r.artist.Name,
                        RecordId = r.record.RecordId,
                        Name = r.record.Name,
                        Recorded = r.record.Recorded,
                        Rating = r.record.Rating,
                        Media = r.record.Media
                    };

                    artistRecords.Add(ar);
                }
            }

            return artistRecords;
        }

        public static int SumOfMissingReviews()
        {
            int total = 0;

            using (var context = new RecordDbContext())
            {
                var count = context.Records
                           .Where(r => string.IsNullOrEmpty(r.Review))
                           .Count();

                total = count;
            }

            return total;
        }

        /// <summary>
        /// Get total number of discs for each artist.
        /// </summary>
        public static IEnumerable<dynamic> GetTotalArtistDiscs()
        {
            var list = new List<dynamic>();

            using (var context = new RecordDbContext())
            {
                return context.Artists
                          .OrderBy(a => a.LastName)
                          .ThenBy(a => a.FirstName)
                          .Select(artist => new
                          {
                              ArtistName = artist.Name,
                              Discs = context.Records.Where(r => r.ArtistId == artist.ArtistId).Sum(r => r.Discs)
                          })
                          .ToList();
            }
        }

        /// <summary>
        /// Get total cost spent on each artist.
        /// </summary>
        public static IEnumerable<dynamic> GetCostTotals()
        {
            var list = new List<dynamic>();

            using (var context = new RecordDbContext())
            {
                return context.Artists
                          .OrderBy(a => a.LastName)
                          .ThenBy(a => a.FirstName)
                          .Select(artist => new
                          {
                              ArtistName = artist.Name,
                              Cost = context.Records.Where(r => r.ArtistId == artist.ArtistId).Sum(r => r.Cost)
                          })
                          .ToList();
            }
        }
    }
}
