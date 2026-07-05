namespace smaertPark.Models
{
    public class GlobalConfig
    {
        public string ParkingLotName { get; set; }
        public string Address { get; set; }
        public int TotalSpots { get; set; }
        public int Floors { get; set; }
        public decimal PricePerHour { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public string Currency { get; set; }
        public int MaxParkingHours { get; set; }
        public bool LicensePlateRecognitionEnabled { get; set; }
    }
}
