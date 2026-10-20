using smartPark.Models;

namespace smartPark.Repository.Interfaces
{
    public interface IParkingSpotRepository
    {
        List<ParkingSpot> GetAll();
        ParkingSpot GetById(int id);
        List<ParkingSpot> GetAvailable();
        List<ParkingSpot> GetByFloor(int floor);
        void Update(ParkingSpot spot);
        void UpdateStatus(int spotId, SpotStatus status, int? vehicle);
        bool Any();
        int Add(ParkingSpot spot);
     void AddRange(List<ParkingSpot> spots);
        void Delete(int id);
    }
}


    