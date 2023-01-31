using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecordDBEFSQLite.Data;

public partial class RecordDbContext : DbContext
{
    public RecordDbContext()
    {
    }

    public RecordDbContext(DbContextOptions<RecordDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["RecordDB"].ConnectionString;
        optionsBuilder.UseSqlite(constr);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("Artist");

            entity.Property(e => e.ArtistId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.ToTable("Record");

            entity.Property(e => e.RecordId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
