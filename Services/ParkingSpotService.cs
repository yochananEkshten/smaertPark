using smartPark.Services.Interfaces;
using smartPark.Models;
using smartPark.Repository.Interfaces;


namespace smartPark.Services
{
    public class ParkingSpotService : IParkingSpotService
    {
        private readonly IParkingSpotRepository _parkingSpotRepository;
        private readonly ILogger<ParkingSpotService> _logger;

        public ParkingSpotService(IParkingSpotRepository parkingSpotRepository, ILogger<ParkingSpotService> logger)
        {
            _parkingSpotRepository = parkingSpotRepository;
            _logger = logger;
        }

        // שליפת כל המקומות
        public List<ParkingSpot> GetAll()
        {
            return _parkingSpotRepository.GetAll();
        }

        // שליפת מקומות פנויים
        public List<ParkingSpot> GetAvailable()
        {
            return _parkingSpotRepository.GetAvailable();
        }

        // שליפת מקומות לפי קומה
        public List<ParkingSpot> GetByFloor(int floor)
        {
            return _parkingSpotRepository.GetByFloor(floor);
        }

        // שליפת מקום לפי Id
        public ParkingSpot GetById(int id)
        {
            return _parkingSpotRepository.GetById(id);
        }

        // הקצאת מקום לרכב – מחזיר את המקום שהוקצה
        public ParkingSpot AssignSpot(int vehicle)
        {
            var availableSpots = _parkingSpotRepository.GetAvailable();
            if (!availableSpots.Any())
            {
                _logger.LogWarning("No available parking spots");
                return null;
            }

            // בחר את המקום הראשון הפנוי
            var spot = availableSpots.First();
            _parkingSpotRepository.UpdateStatus(spot.Id, SpotStatus.Occupied, vehicle);
            return spot;
        }

        // שחרור מקום חניה
        public void FreeSpot(int spotId)
        {
            _parkingSpotRepository.UpdateStatus(spotId, SpotStatus.Available, null);
        }
    }
}
