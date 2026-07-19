
using smaertPark.Models;

namespace smaertPark.Repository.Interfaces
{
    public interface IVehicleEntryRepository
    {
        VehicleEntry GetByLicensePlate(string licensePlate);
        VehicleEntry GetActiveByLicensePlate(string licensePlate);
        List<VehicleEntry> GetAll();
        List<VehicleEntry> GetActive();
        VehicleEntry GetById(int id);
        int Add(VehicleEntry vehicle);
        void Update(VehicleEntry vehicle);
        void Exit(int id);
    }
}

