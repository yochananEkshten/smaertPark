using smartPark.Models;

namespace smartPark.Services.Interfaces
{
    public interface IParkingSpotService
    {
        List<ParkingSpot> GetAll();
        List<ParkingSpot> GetAvailable();
        List<ParkingSpot> GetByFloor(int floor);
        ParkingSpot GetById(int id);
        ParkingSpot AssignSpot(int vehicle);
        void FreeSpot(int spotId);
    }
}
