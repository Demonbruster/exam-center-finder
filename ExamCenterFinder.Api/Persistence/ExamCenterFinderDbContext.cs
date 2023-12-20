using ExamCenterFinder.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExamCenterFinder.Api.Persistence
{
    public class ExamCenterFinderDbContext: DbContext
    {
        public ExamCenterFinderDbContext(DbContextOptions<ExamCenterFinderDbContext> options) : base(options)
        {
            var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (dbCreator != null)
            {
                // Create Database 
                if (!dbCreator.CanConnect())
                {
                    dbCreator.Create();
                }

                // Create Tables
                if (!dbCreator.HasTables())
                {
                    dbCreator.CreateTables();
                }
            }
        }

        public DbSet<ExamCenter> ExamCenters { get; set; }
        public DbSet<ZipCodeCenterPoint> ZipCodeCenterPoints { get; set; }
        public DbSet<ExamSlot> ExamSlots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed ZipCodeCenterPoints
            modelBuilder.Entity<ZipCodeCenterPoint>().HasData(
                new ZipCodeCenterPoint { Id = 1, ZipCode = "11111", Latitude = 45.22738570006638, Longitude = -93.9960240952021 },
                new ZipCodeCenterPoint { Id = 2, ZipCode = "22222", Latitude = 45.56189444715879, Longitude = -93.22693539547762 },
                new ZipCodeCenterPoint { Id = 3, ZipCode = "33333", Latitude = 44.84908604562181, Longitude = -92.23998199472152 }
            );

            // Seed ExamCenters and ExamSlots
            modelBuilder.Entity<ExamCenter>().HasData(
                new ExamCenter
                {
                    Id = 1,
                    Name = "ABC Testing Center",
                    StreetAddress = "123 Main St, Minneapolis, MN 12345",
                    ZipCodeCenterPointId = 1,
                    City = "City 1",
                    State = "State 1"
                },
                new ExamCenter
                {
                    Id = 2,
                    Name = "DEF Testing Center",
                    StreetAddress = "456 Side St, Minneapolis, MN 54321",
                    ZipCodeCenterPointId = 2,
                    City = "City 2",
                    State = "State 2"
                },
                new ExamCenter
                {
                    Id = 3,
                    Name = "GHI Testing Center",
                    StreetAddress = "789 Cross St, Minneapolis, MN 45123",
                    ZipCodeCenterPointId = 3,
                    City = "City 3",
                    State = "State 3"
                }
            );

            modelBuilder.Entity<ExamSlot>().HasData(
                new ExamSlot
                {
                    Id = 1,
                    StartTime = DateTime.Parse("2023-05-01T15:00:00"),
                    Duration = 2,
                    TotalSeats = 115,
                    ReservedSeats = 5,
                    ExamCenterId = 1, // Foreign key value for 'ABC Testing Center'
                },
                new ExamSlot
                {
                    Id = 2,
                    StartTime = DateTime.Parse("2023-05-02T13:30:00"),
                    Duration = 2,
                    TotalSeats = 115,
                    ReservedSeats = 10,
                    ExamCenterId = 2, // Foreign key value for 'DEF Testing Center'
                },
                new ExamSlot
                {
                    Id = 3,
                    StartTime = DateTime.Parse("2023-05-01T10:30:00"),
                    Duration = 2,
                    TotalSeats = 115,
                    ReservedSeats = 1,
                    ExamCenterId = 3, // Foreign key value for 'GHI Testing Center'
                }
            );

        }
    }
}
