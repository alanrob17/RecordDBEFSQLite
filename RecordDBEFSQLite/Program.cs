using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RecordDBEFSQLite.Data;
using System.Collections.Generic;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using _at = RecordDBEFSQLite.Tests.ArtistTest;
using _rt = RecordDBEFSQLite.Tests.RecordTest;

namespace RecordDBEFSQLite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //// Artists
            // _at.CreateArtist();
            // _at.GetArtistById(114);
            // _at.GetArtistEntity(114);
            // _at.GetArtistByName("Bruce Cockburn");
            // _at.GetBiography(114);
            // _at.ArtistHtml(114);
            // _at.GetArtistId("Bob", "Dylan");
            // _at.UpdateArtist(823);
            // _at.DeleteArtist(823);
            // _at.GetArtist(823);
            // _at.GetArtistName(114);
            // _at.GetArtistNames();
            // _at.GetArtistsWithNoBio();
            // _at.GetNoBiographyCount();

            //// Records
            // _rt.CreateRecord(823);
            // _rt.UpdateRecord(5251);
            // _rt.DeleteRecord(5251);
            // _rt.GetRecordById(2196);
            // _rt.GetRecordEntity(2196);
            // _rt.GetArtistRecordByYear(1974);
            // _rt.GetRecordList();
            // _rt.GetRecordList2();
            // _rt.GetRecordList3();
            // _rt.CountDiscs(string.Empty);
            // _rt.CountDiscs("DVD");
            // _rt.CountDiscs("CD");
            // _rt.CountDiscs("R");
            // _rt.GetArtistRecordEntity(2196);
            // _rt.GetArtistNumberOfRecords(114);
            // _rt.GetRecordDetails(2196);
            // _rt.GetArtistNameFromRecord(2196);
            // _rt.GetDiscCountForYear(2010);
            // _rt.GetBoughtDiscCountForYear(2018);
            // _rt.GetNoRecordReview();
            // _rt.GetNoReviewCount();
            // _rt.GetTotalArtistCost();
            // _rt.GetTotalArtistDiscs();
            // _rt.RecordHtml(2196);
        }
    }
}
