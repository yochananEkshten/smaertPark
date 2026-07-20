using smartPark.Data;
using smartPark.Models;
using smartPark.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace smartPark.Repository
{
    public class DriverRepository : IDriverRepository
    {
        private readonly IDbContextFactory<ParkingDbContext> _factory;
        private readonly ILogger<DriverRepository> _logger;

        public DriverRepository(IDbContextFactory<ParkingDbContext> factory, ILogger<DriverRepository> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        // שליפת נהג לפי לוחית רישוי
        public Driver GetByLicensePlate(string licensePlate)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                return context.Drivers
                    .FirstOrDefault(d => d.LicensePlate == licensePlate
                                     && d.RecordStatus == 1);
            }
        }

        // שליפת נהג לפי Id
        public Driver GetById(int id)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                return context.Drivers
                    .FirstOrDefault(d => d.Id == id && d.RecordStatus == 1);
            }
        }

        // שליפת כל הנהגים
        public List<Driver> GetAll()
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                return context.Drivers
                    .Where(d => d.RecordStatus == 1)
                    .ToList();
            }
        }

        // הוספת נהג חדש
        public int Add(Driver driver)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                driver.CreatedAt = DateTime.UtcNow;
                driver.RecordStatus = 1;
                context.Drivers.Add(driver);
                context.SaveChanges();
                return driver.Id;
            }
        }

        // עדכון נהג
        public void Update(Driver driver)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                context.Drivers.Update(driver);
                context.SaveChanges();
            }
        }

        // מחיקה רכה (Soft Delete)
        public void Delete(int id)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                var driver = context.Drivers.FirstOrDefault(d => d.Id == id);
                if (driver != null)
                {
                    driver.RecordStatus = 2; // 2 = מחוק
                    context.SaveChanges();
                }
            }
        }
    }
}
