
using Microsoft.EntityFrameworkCore;
using smartPark.Data;
using smartPark.Models;
using smartPark.Repository.Interfaces;
#nullable disable
namespace smartPark.Repository
{
    public class VehicleEntryRepository : IVehicleEntryRepository
    {
        private readonly IDbContextFactory<ParkingDbContext> _factory;
        private readonly ILogger<VehicleEntryRepository> _logger;

        public VehicleEntryRepository(IDbContextFactory<ParkingDbContext> factory, ILogger<VehicleEntryRepository> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        // שליפת רכב לפי לוחית רישוי
        public VehicleEntry GetByLicensePlate(string licensePlate)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                var vehicle = context.VehicleEntries
                    .FirstOrDefault(v => v.PlateNumber == licensePlate);

                if (vehicle == null)
                {
                    _logger.LogWarning($"Vehicle {licensePlate} not found");
                    return null;
                }
                return vehicle;
            }
        }

        // שליפת רכב פעיל (בחניון כרגע) לפי לוחית רישוי
        public VehicleEntry GetActiveByLicensePlate(string licensePlate)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                var vehicle = context.VehicleEntries
                    .FirstOrDefault(v => v.PlateNumber == licensePlate
                                     && v.Status == VehicleStatus.Inside);

                if (vehicle == null)
                {
                    _logger.LogWarning($"Active vehicle {licensePlate} not found");
                    return null;
                }

                return vehicle;
            }
        }
        

        // שליפת כל הרכבים
        public List<VehicleEntry> GetAll()
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                return context.VehicleEntries
                    .Include(v => v.Spot)
                    .ToList();
            }
        }

        // שליפת רכבים פעילים (בחניון כרגע)
        public List<VehicleEntry> GetActive()
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                return context.VehicleEntries
                    .Include(v => v.Spot)
                    .Where(v => v.Status == VehicleStatus.Inside)
                    .ToList();
            }
        }

        // שליפת רכב לפי Id
        public VehicleEntry GetById(int id)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                var vehicle = context.VehicleEntries
                    .Include(v => v.Spot)
                    .FirstOrDefault(v => v.Id == id);

                if (vehicle == null)
                {
                    _logger.LogWarning($"Vehicle with id {id} not found");
                    return null;
                }

                return vehicle;
            }
        }
      
        // הוספת רכב חדש
        public int Add(VehicleEntry vehicle)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                vehicle.EntryTime = DateTime.UtcNow;
                vehicle.Status = VehicleStatus.Inside;
                context.VehicleEntries.Add(vehicle);
                context.SaveChanges();
                return vehicle.Id;
            }
        }

        // עדכון רכב
        public void Update(VehicleEntry vehicle)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                context.VehicleEntries.Update(vehicle);
                context.SaveChanges();
            }
        }

        // יציאת רכב מהחניון
        public void Exit(int id)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                var vehicle = context.VehicleEntries.FirstOrDefault(v => v.Id == id);
                if (vehicle != null)
                {
                    vehicle.ExitTime = DateTime.UtcNow;
                    vehicle.Status = VehicleStatus.Exited;
                    vehicle.SpotId = null;
                    context.SaveChanges();
                }
            }
        }
    }
}

