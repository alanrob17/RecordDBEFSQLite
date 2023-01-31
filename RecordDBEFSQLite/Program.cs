using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RecordDBEFSQLite.Data;
using System.Collections.Generic;
using System.Data;
using _ad = RecordDBEFSQLite.Data.ArtistData;
using _rd = RecordDBEFSQLite.Data.RecordData;

namespace RecordDBEFSQLite
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // GetArtistNames();
            // GetRecordList();
            // GetRecordList2();
            // GetRecordList3();
            // GetArtistRecordByYear(1974);
        }

        // Artist - Record list - better code.
        private static void GetRecordList()
        {
            var artistRecords = _ad.GetArtists().
                GroupJoin(
                    _rd.GetRecords(),
                    a => a.ArtistId,
                    r => r.ArtistId,
                    (a, r) => new { a, r }
                );

            foreach (var artist in artistRecords)
            {
                Console.WriteLine($"\n{artist.a.Name}\n");

                foreach (var record in artist.r)
                {
                    Console.WriteLine($"\t{record.Recorded} - {record.Name} ({record.Media})");
                }
            }
        }

        // Artist - Record list.
        private static void GetRecordList2()
        {
            var artists = _ad.GetArtists();
            var records = _rd.GetRecords();

            foreach (var artist in artists)
            {
                Console.WriteLine($"{artist.Name}:\n");

                var ar = from r in records
                         where artist.ArtistId == r.ArtistId
                         orderby r.Recorded descending
                         select r;

                foreach (var rec in ar)
                {
                    Console.WriteLine($"\t{rec.Recorded} - {rec.Name} ({rec.Media})");
                }

                Console.WriteLine();
            }
        }

        // All records in single row.
        private static void GetRecordList3()
        {
            var list = _rd.GetRecordList();

            foreach (var record in list)
            {
                Console.WriteLine($"{record.Artist}: {record.Name} - {record.Recorded} - {record.Media}");
            }
        }

        private static void GetArtistRecordByYear(int year)
        {
            var list = _rd.GetArtistRecordByYear(year);
            Console.WriteLine($"List of records for {year}.");

            foreach (var record in list)
            {
                Console.WriteLine($"{record.Artist}: {record.Name} - {record.Recorded} - {record.Media}");
            }
        }

        private static void GetArtistNames()
        {
            var artists = _ad.GetArtists();
            foreach (var artist in artists)
            {
                Console.WriteLine($"{artist.ArtistId} - {artist.Name}");
            }
        }


    }
}