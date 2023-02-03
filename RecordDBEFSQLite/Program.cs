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
            // GetArtistById(114);
            // GetArtistByName("Bruce Cockburn");
            // GetBiography(114);
            // ArtistHtml(114);
            // GetArtistId("Bob", "Dylan");
            // CreateArtist();
            // UpdateArtist(823);
            // DeleteArtist(823);
            // GetArtist(63);
            // GetArtistName(114);
            // GetArtistNames();
            // GetArtistsWithNoBio();
            // GetNoBiographyCount();
            // GetArtistRecordByYear(1974);
            // GetRecordList();
            // GetRecordList2();
            // GetRecordList3();
        }

        private static void GetBiography(int artistid)
        {
            var biography = _ad.GetBiography(artistid);

            if (biography.Length > 5)
            {
                Console.WriteLine(biography);
            }
        }

        private static void ArtistHtml(int artistId)
        {
            var artist = _ad.ShowArtist(artistId);

            Console.WriteLine(artist);
        }

        private static void GetArtistId(string firstName, string lastName)
        {
            int artistId = _ad.GetArtistId(firstName, lastName);

            if (artistId > 0)
            {
                Console.WriteLine($"Artist Id is {artistId}");
            }
            else 
            { 
                Console.WriteLine("ERROR: Artist name not found.");
            }
        }

        private static void DeleteArtist(int artistId)
        {
            var success = _ad.DeleteArtist(artistId);

            if (success)
            {
                Console.WriteLine($"Artist with Id: {artistId} deleted.");
            }
            else
            {
                Console.WriteLine($"ERROR: Couldn't delete Artist with Id: {artistId}!");
            }
        }

        private static void UpdateArtist(int artistId)
        {
            Artist artist = new()
            {
                ArtistId = artistId,
                FirstName = "Joseph",
                LastName = "Whopposoni",
                Name = "",
                Biography = "Joe is an Italian country and western singer. He likes both kinds of music."
            };

            bool success = _ad.UpdateArtist(artist);

            if (success)
            {
                Console.WriteLine($"Successfully update artist with Id: {artistId}");
            }
            else 
            {
                Console.WriteLine("ERROR: Artist couldn't be updated!");
            }
        }

        private static void CreateArtist()
        {
            Artist artist = new()
            {
                FirstName = "Joe",
                LastName = "Whoppo",
                Name = "",
                Biography = "Joe is a country and western singer. He likes both kinds of music."
            };

            artist.Name = string.IsNullOrEmpty(artist.FirstName) ? artist.LastName : $"{artist.FirstName} {artist.LastName}";

            var exists = _ad.CheckForArtistName(artist.Name);

            if (exists)
            {
                Console.WriteLine("Artist Already exists in the Database!");
            }
            else
            {
                var id = _ad.InsertArtist(artist);

                if (id > 0)
                {
                    Console.WriteLine($"Artist created with Id: {id}");
                }
                else
                {
                    Console.WriteLine("New artist record not created!");
                }
            }
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

        /// <summary>
        /// Get Artist details.
        /// </summary>
        public static void GetArtist(int artistId)
        {
            var artist = _ad.GetArtist(artistId);

            if (artist.ArtistId > 0)
            {
                Console.WriteLine($"Artist Id: {artist.ArtistId}.\nName: {artist?.Name},\n Biography: {artist?.Biography}");
            }
            else
            {
                Console.WriteLine($"Artist with Id: {artistId} not found!");
            }
        }

        /// <summary>
        /// Get Artist name.
        /// </summary>
        public static void GetArtistName(int artistId)
        {
            var artist = _ad.GetArtist(artistId);

            if (artist.ArtistId > 0)
            {
                Console.WriteLine(artist.Name);
            }
            else
            {
                Console.WriteLine($"Artist with Id: {artistId} not found!");
            }
        }

        /// <summary>
        /// Select all artists with no biography.
        /// </summary>
        public static void GetArtistsWithNoBio()
        {
            var artists = _ad.GetArtistsWithNoBio();

            if (artists.Any())
            {
                foreach (var artist in artists)
                {
                    Console.WriteLine($"Id: {artist.ArtistId} - {artist.Name}");
                }
            }
        }

        /// <summary>
        /// Get the count of Artist records with no Biography.
        /// </summary>
        private static void GetNoBiographyCount()
        {
            var count = _ad.NoBiographyCount();

            if (count > 0)
            {
                Console.WriteLine($"The number of records with no artist biography is: {count}.");
            }
        }

        public static void GetArtistByName(string artistName)
        {

            var artist = _ad.GetArtistByName(artistName);

            Console.WriteLine(artist);
        }

        private static void GetArtistById(int artistId)
        {
            var artistRecords = _ad.GetArtistById(artistId);

            Console.WriteLine(artistRecords);
        }
    }
}