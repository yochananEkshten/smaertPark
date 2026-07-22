
using Microsoft.EntityFrameworkCore;
using smartPark.Data;
using smartPark.Models;
using smartPark.Repository.Interfaces;

namespace smartPark.Repository
{
    public class GlobalConfigRepository : IGlobalConfigRepository
    {
        private readonly IDbContextFactory<ParkingDbContext> _factory;
        private readonly ILogger<GlobalConfigRepository> _logger;

        public GlobalConfigRepository(IDbContextFactory<ParkingDbContext> factory, ILogger<GlobalConfigRepository> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        // שליפת הגדרות החניון
        public GlobalConfig Get()
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                return context.GlobalConfigs.FirstOrDefault();
            }
        }

        // עדכון הגדרות החניון
        public void Update(GlobalConfig config)
        {
            using (ParkingDbContext context = _factory.CreateDbContext())
            {
                context.GlobalConfigs.Update(config);
                context.SaveChanges();
            }
        }
    }
}
