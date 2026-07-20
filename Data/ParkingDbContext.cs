using Microsoft.EntityFrameworkCore;
using smartPark.Models;
using System.Collections.Generic;

using System.Reflection.Emit;

namespace smartPark.Data
{
    public class ParkingDbContext : DbContext
    {
        public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
      
        public DbSet<VehicleEntry> VehicleEntries { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<GlobalConfig> GlobalConfigs { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        

            // הגדרות כלליות ראשוניות
            modelBuilder.Entity<GlobalConfig>().HasData(
                new GlobalConfig
                {
                    Id = 1,
                    ParkingLotName = "חניון מרכזי",
                    Address = "רחוב הרצל 1, תל אביב",
                    ScreenTitle = "ברוכים הבאים לחניון המרכזי",
                    TotalSpots = 150,
                    Floors = 3,
                    OpeningTime = new TimeSpan(6, 0, 0),
                    ClosingTime = new TimeSpan(23, 0, 0),
                    PricePerHourBusiness = 15,
                    PricePerHourRegular = 8,
                    BusinessHoursStart = new TimeSpan(8, 0, 0),
                    BusinessHoursEnd = new TimeSpan(18, 0, 0),
                    Currency = "ILS",
                    MaxParkingHours = 24,
                    LicensePlateRecognitionEnabled = true
                }
            );

            // ======= Unique Constraints =======
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName).IsUnique();

            modelBuilder.Entity<VehicleEntry>()
                .HasIndex(v => v.PlateNumber).IsUnique();

            modelBuilder.Entity<ParkingSpot>()
                .HasIndex(p => p.SpotNumber).IsUnique();
               modelBuilder.Entity<Driver>()
              .HasIndex(d => d.LicensePlate).IsUnique();

            // ======= Relationships =======

           

            // VehicleEntry -> ParkingSpot
            modelBuilder.Entity<VehicleEntry>()
                .HasOne(v => v.Spot)
                .WithOne(p => p.Vehicle)
                .HasForeignKey<VehicleEntry>(v => v.SpotId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
