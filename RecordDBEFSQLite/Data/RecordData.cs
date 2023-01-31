using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
