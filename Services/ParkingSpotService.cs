using smartPark.Services.Interfaces;
using smartPark.Models;
using smartPark.Repository.Interfaces;



namespace smartPark.Services
{
    public class ParkingSpotService : IParkingSpotService
    {
        private readonly IParkingSpotRepository _parkingSpotRepository;
        private readonly IGlobalConfigService _globalConfigService;
        private readonly ILogger<ParkingSpotService> _logger;

        public ParkingSpotService(
            IParkingSpotRepository parkingSpotRepository,
            IGlobalConfigService globalConfigService,
            ILogger<ParkingSpotService> logger)
        {
            _parkingSpotRepository = parkingSpotRepository;
            _globalConfigService = globalConfigService;
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

        // יצירת מקומות חניה לפי GlobalConfig (TotalSpots, Floors)
        // פעולה חד-פעמית: אם כבר יש spots בטבלה – זורקת שגיאה ולא יוצרת כפילויות
        public List<ParkingSpot> GenerateSpots()
        {
            if (_parkingSpotRepository.Any())
            {
                _logger.LogWarning("GenerateSpots was called, but parking spots already exist");
                throw new InvalidOperationException(
                    "כבר קיימים מקומות חניה במערכת. יש למחוק אותם קודם אם רוצים ליצור מחדש.");
            }

            var config = _globalConfigService.Get();
            if (config == null)
            {
                throw new InvalidOperationException("לא נמצאה הגדרת GlobalConfig – אי אפשר לייצר מקומות.");
            }

            if (config.TotalSpots <= 0 || config.Floors <= 0)
            {
                throw new InvalidOperationException("TotalSpots ו-Floors ב-GlobalConfig חייבים להיות גדולים מ-0.");
            }

            var spots = new List<ParkingSpot>();

            // חלוקה שווה בין הקומות; השארית (אם TotalSpots לא מתחלק ב-Floors) מתווספת לקומות הראשונות
            int baseSpotsPerFloor = config.TotalSpots / config.Floors;
            int remainder = config.TotalSpots % config.Floors;

            for (int floor = 1; floor <= config.Floors; floor++)
            {
                int spotsOnThisFloor = baseSpotsPerFloor + (floor <= remainder ? 1 : 0);

                for (int index = 1; index <= spotsOnThisFloor; index++)
                {
                    spots.Add(new ParkingSpot
                    {
                        SpotNumber = $"{floor}-{index}",
                        Floor = floor,
                        StatusId = SpotStatus.Available
                    });
                }
            }

            _parkingSpotRepository.AddRange(spots);
            _logger.LogInformation($"Generated {spots.Count} parking spots across {config.Floors} floors");

            return spots;
        }

        // הוספת מקום בודד (StatusId מגיע מה-request – מאפשר גם להוסיף מקום כ-Disabled ישירות)
        public int Add(ParkingSpot spot)
        {
            return _parkingSpotRepository.Add(spot);
        }

        // עדכון מקום חניה
        public void Update(ParkingSpot spot)
        {
            _parkingSpotRepository.Update(spot);
        }

        // מחיקת מקום חניה
        public void Delete(int id)
        {
            _parkingSpotRepository.Delete(id);
        }
    }
}
