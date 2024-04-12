using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SD.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Persistence.Repositories.DBContext
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext() { }

        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) {

            Database.SetCommandTimeout(90);
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<MediumType> MediumTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable(nameof(Movie) + "s");
                /* entity.HasKey(e => e.Id); Schlüssel definieren - hier nicht notwendig, da implizit */
                entity.Property(p => p.Title).IsRequired().HasMaxLength(128);
                entity.HasIndex(p => p.Title).HasDatabaseName("IX_" + nameof(Movie) + "s_" + nameof(Movie.Title));
                entity.Property(p => p.ReleaseDate).HasColumnType("date");
                entity.Property(p => p.Price).HasPrecision(18, 2).HasDefaultValue(0M);
            });

            modelBuilder.Entity<MediumType>(entity =>
            {
                entity.ToTable(nameof(MediumType) + "s").HasKey(nameof(MediumType.Code));   
            });

            /* Foreign Key Constraint 0 : n */
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.MediumType)
                .WithMany(m => m.Movies)
                .HasForeignKey(m => m.MediumTypeCode)
                .OnDelete(DeleteBehavior.SetNull).IsRequired(false); /* Löschweitergabe => Wert in Movie auf Null setzen */


            /* Foreign Key Constraing 1 : n */
            modelBuilder.Entity<Genre>().ToTable(nameof(Genre) + "s")
                .HasMany(g => g.Movies)
                .WithOne(g => g.Genre)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.Restrict); /* Keine Löschweitergabe */

            /* Seed - Default Daten schreiben */

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action"},
                new Genre { Id = 2, Name = "Horror" },
                new Genre { Id = 3, Name = "Science Fiction" },
                new Genre { Id = 4, Name = "Comedy" }
            );

            modelBuilder.Entity<MediumType>().HasData(
                new MediumType { Code = "VHS", Name = "Videokassette" },
                new MediumType { Code = "DVD", Name = "Digital Versitale Disc" },
                new MediumType { Code = "BR", Name = "Blu Ray" },
                new MediumType { Code = "BR3D", Name = "3D Blu Ray" },
                new MediumType { Code = "BRHD", Name = "HD Blu Ray" },
                new MediumType { Code = "BR4K", Name = "4K Blu Ray" }
            );

            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = new Guid("746f7276-2e73-4012-b820-2651c26b3824"),
                    Title = "Rambo",
                    Price = 4.9M,
                    MediumTypeCode = "VHS",
                    ReleaseDate = new DateTime(1985, 4, 13),
                    GenreId = 1
                },

                new Movie
                {
                    Id = new Guid("3ce3f159-c611-4d21-be1c-25311c76bf1a"),
                    Title = "Star Trek - Beyond",
                    GenreId = 3,                    
                    MediumTypeCode = "BR3D",
                    ReleaseDate = new DateTime(2016, 5, 30),
                    Price = 14.9M
                },

                new Movie
                {
                    Id = new Guid("1e045441-dc32-4b62-9176-57e783a42ff6"),
                    Title = "Star Wars - Episode IV",
                    GenreId = 3,
                    MediumTypeCode = "DVD",
                    ReleaseDate = new DateTime(1987, 4, 13),
                    Price = 9.9M
                },

                new Movie
                {
                    Id = new Guid("39492302-d578-43a9-97fa-450312352c41"),
                    Title = "The Ring",
                    GenreId = 2,
                    MediumTypeCode = "BR",
                    ReleaseDate = new DateTime(2005, 11, 15),
                    Price = 12.9M
                }

            );

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
#if DEBUG
            /* Verzeichnis bis \bin kürzen */
            if(currentDirectory.IndexOf("bin") > -1)
            {
                currentDirectory = currentDirectory.Substring(0, currentDirectory.IndexOf("bin"));
            }
#endif

            var configurationBuilder = new ConfigurationBuilder()
                                            .SetBasePath(currentDirectory)
                                            .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);

            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString(nameof(MovieDbContext));
            optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout(60));
        }
    }


    /* Command für Initialisierung der EF-Migration 
     * 1. PM Console öffnen
     * 2. add-migration Initial -startupProject SD.Persistence
     * 3. update-database -startupProject SD.Persistence
     */
}

