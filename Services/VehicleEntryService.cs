using smartPark.Services.Interfaces;
using smartPark.Models;
using smartPark.Repository.Interfaces;



namespace smartPark.Services
{
    public class VehicleEntryService : IVehicleEntryService
    {
        private readonly IVehicleEntryRepository _vehicleEntryRepository;
        private readonly IParkingSpotService _parkingSpotService;
        private readonly IGlobalConfigService _globalConfigService;
        private readonly ILogger<VehicleEntryService> _logger;

        public VehicleEntryService(
            IVehicleEntryRepository vehicleEntryRepository,
            IParkingSpotService parkingSpotService,
            IGlobalConfigService globalConfigService,
            ILogger<VehicleEntryService> logger)
        {
            _vehicleEntryRepository = vehicleEntryRepository;
            _parkingSpotService = parkingSpotService;
            _globalConfigService = globalConfigService;
            _logger = logger;
        }

        // שליפת כל הרכבים
        public List<VehicleEntry> GetAll()
        {
            return _vehicleEntryRepository.GetAll();
        }

        // שליפת רכבים פעילים
        public List<VehicleEntry> GetActive()
        {
            return _vehicleEntryRepository.GetActive();
        }

        // שליפת רכב לפי Id
        public VehicleEntry GetById(int id)
        {
            return _vehicleEntryRepository.GetById(id);
        }

        // שליפת רכב פעיל לפי לוחית רישוי
        public VehicleEntry GetActiveByLicensePlate(string licensePlate)
        {
            return _vehicleEntryRepository.GetActiveByLicensePlate(licensePlate);
        }

        // כניסת רכב לחניון
        public int VehicleEntry(string licensePlate, int? driverId)
        {
            // בדוק אם הרכב כבר בחניון
            var existing = _vehicleEntryRepository.GetActiveByLicensePlate(licensePlate);
            if (existing != null)
            {
                _logger.LogWarning($"Vehicle {licensePlate} is already in the parking lot");
                return existing.Id;
            }

            // הקצה מקום חניה
            var vehicle = new Models.VehicleEntry
            {
                PlateNumber = licensePlate,
                DriverId = driverId,
                EntryTime = DateTime.UtcNow,
                Status = VehicleStatus.Inside
            };

            int vehicleId = _vehicleEntryRepository.Add(vehicle);

            // עדכן מקום חניה
            _parkingSpotService.AssignSpot(vehicleId);

            return vehicleId;
        }

        // יציאת רכב מהחניון + חישוב תשלום
        public decimal VehicleExit(int id)
        {
            var vehicle = _vehicleEntryRepository.GetById(id);
            if (vehicle == null) return 0;

            // חשב תשלום
            decimal payment = CalculatePayment(vehicle.EntryTime, DateTime.UtcNow);

            // שחרר מקום חניה
            if (vehicle.SpotId.HasValue)
                _parkingSpotService.FreeSpot(vehicle.SpotId.Value);

            // עדכן יציאה
            _vehicleEntryRepository.Exit(id);

            return payment;
        }

        // חישוב תשלום לפי תעריף
        private decimal CalculatePayment(DateTime entryTime, DateTime exitTime)
        {
            var config = _globalConfigService.Get();
            double totalHours = (exitTime - entryTime).TotalHours;

            var businessStart = config.BusinessHoursStart;
            var businessEnd = config.BusinessHoursEnd;
            var now = exitTime.TimeOfDay;

            // בדוק אם שעות עסקים
            bool isBusinessHours = now >= businessStart && now <= businessEnd;

            decimal pricePerHour = isBusinessHours
                ? config.PricePerHourBusiness
                : config.PricePerHourRegular;

            return Math.Round((decimal)totalHours * pricePerHour, 2);
        }
    }
}
