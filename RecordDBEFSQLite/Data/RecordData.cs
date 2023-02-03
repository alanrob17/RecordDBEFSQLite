using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

        public static List<ArtistRecord> GetRecordList()
        {
            List<ArtistRecord> list = new List<ArtistRecord>();

            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .OrderBy(r => r.artist.LastName)
                    .ThenBy(r => r.artist.FirstName)
                    .ThenBy(r => r.record.Recorded)
                    .ToList();

                if (records.Any())
                {
                    foreach (var r in records)
                    {
                        ArtistRecord ar = new ArtistRecord();
                        ar.ArtistId = r.artist.ArtistId;
                        ar.Artist = r.artist.Name;
                        ar.RecordId = r.record.RecordId;
                        ar.Name = r.record.Name;
                        ar.Recorded = r.record.Recorded;
                        ar.Rating = r.record.Rating;
                        ar.Media = r.record.Media;
                        list.Add(ar);
                    }
                }
            }

            return list;
        }

        public static List<ArtistRecord> GetArtistRecordByYear(int year)
        {
            List<ArtistRecord> list = new List<ArtistRecord>();

            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .Where(r => r.record.Recorded == year)
                    .OrderBy(r => r.artist.LastName)
                    .ThenBy(r => r.artist.FirstName)
                    .ToList();

                if (records.Any())
                {
                    foreach (var r in records)
                    {
                        ArtistRecord ar = new ArtistRecord();
                        ar.ArtistId = r.artist.ArtistId;
                        ar.Artist = r.artist.Name;
                        ar.RecordId = r.record.RecordId;
                        ar.Name = r.record.Name;
                        ar.Recorded = r.record.Recorded;
                        ar.Rating = r.record.Rating;
                        ar.Media = r.record.Media;
                        list.Add(ar);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Get record details including the artist name.
        /// </summary>
        public static ArtistRecord GetArtistRecordEntity(int recordId)
        {
            var ar = new ArtistRecord();

            using (var context = new RecordDbContext())
            {
                var record = context.Records.FirstOrDefault(r => r.RecordId == recordId);

                if (record is Record)
                {
                    var artist = context.Artists.FirstOrDefault(r => r.ArtistId == record.ArtistId);

                    if (artist is Artist)
                    {
                        ar.Artist = artist.Name;
                        ar.ArtistId = artist.ArtistId;
                    }

                    ar.RecordId = record.RecordId;
                    ar.Name = record.Name;
                    ar.Recorded = record.Recorded;
                    ar.Rating = record.Rating;
                    ar.Media = record.Media;
                }
                else
                {
                    ar.ArtistId = 0;
                    ar.RecordId = 0;
                }

                return ar;
            }
        }
        
        /// <summary>
        /// Get record details including the artist name.
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
                    mediaTypes = new List<string> { "DVD", "CD/DVD", "Blu-ray", "CD/Blu-ray", "CD", "R"};
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
    }
}
