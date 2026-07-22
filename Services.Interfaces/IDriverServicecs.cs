using smartPark.Models;

namespace smartPark.Services.Interfaces
{
    public interface IDriverService
    {
        Driver GetByLicensePlate(string licensePlate);
        Driver GetById(int id);
        List<Driver> GetAll();
        int Add(Driver driver);
        void Update(Driver driver);
        void Delete(int id);
    }
}
