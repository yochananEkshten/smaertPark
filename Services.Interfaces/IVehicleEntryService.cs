using smartPark.Models;


namespace smartPark.Services.Interfaces
{
    public interface IVehicleEntryService
    {
        List<VehicleEntry> GetAll();
        List<VehicleEntry> GetActive();
        VehicleEntry GetById(int id);
        VehicleEntry GetActiveByLicensePlate(string licensePlate);
        int VehicleEntry(string licensePlate, int? driverId);
        decimal VehicleExit(int id);
    }
}

