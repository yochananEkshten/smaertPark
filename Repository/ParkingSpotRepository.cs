
using Microsoft.EntityFrameworkCore;
using smartPark.Data;
using smartPark.Models;
using smartPark.Repository.Interfaces;

namespace smartPark.Repository
{
    public class ParkingSpotRepository : IParkingSpotRepository
    {
        private readonly IDbContextFactory<ParkingDbContext> _factory;
        private readonly ILogger<ParkingSpotRepository> _logger;

        public ParkingSpotRepository(IDbContextFactory<ParkingDbContext> factory, ILogger<ParkingSpotRepository> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        // שליפת כל המקומות
        public List<ParkingSpot> GetAll()
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                return context.ParkingSpots.ToList();
            }
        }

        // שליפת מקום לפי Id
        public ParkingSpot GetById(int id)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                return context.ParkingSpots.FirstOrDefault(p => p.Id == id);
            }
        }

        // שליפת כל המקומות הפנויים
        public List<ParkingSpot> GetAvailable()
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                return context.ParkingSpots
                    .Where(p => p.StatusId == SpotStatus.Available)
                    .ToList();
            }
        }

        // שליפת מקומות לפי קומה
        public List<ParkingSpot> GetByFloor(int floor)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                return context.ParkingSpots
                    .Where(p => p.Floor == floor)
                    .ToList();
            }
        }

        // עדכון מקום חניה
        public void Update(ParkingSpot spot)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                context.ParkingSpots.Update(spot);
                context.SaveChanges();
            }
        }

        // עדכון סטטוס מקום + רכב
        public void UpdateStatus(int spotId, SpotStatus status, int? vehicle)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                var spot = context.ParkingSpots.FirstOrDefault(p => p.Id == spotId);
                if (spot != null)
                {
                    spot.StatusId = status;
                    spot.VehicleId = vehicle;
                    spot.UpdatedAt = DateTime.UtcNow;
                    context.SaveChanges();
                }
            }
        }
    }
}

