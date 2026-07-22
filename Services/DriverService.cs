using smartPark.Models;
using smartPark.Repository.Interfaces;
using smartPark.Services.Interfaces;



namespace smartPark.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly ILogger<DriverService> _logger;

        public DriverService(IDriverRepository driverRepository, ILogger<DriverService> logger)
        {
            _driverRepository = driverRepository;
            _logger = logger;
        }

        public Driver GetByLicensePlate(string licensePlate)
        {
            return _driverRepository.GetByLicensePlate(licensePlate);
        }

        public Driver GetById(int id)
        {
            return _driverRepository.GetById(id);
        }

        public List<Driver> GetAll()
        {
            return _driverRepository.GetAll();
        }

        public int Add(Driver driver)
        {
            return _driverRepository.Add(driver);
        }

        public void Update(Driver driver)
        {
            _driverRepository.Update(driver);
        }

        public void Delete(int id)
        {
            _driverRepository.Delete(id);
        }
    }
}

