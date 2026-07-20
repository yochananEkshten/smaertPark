using smartPark.Models;



namespace smartPark.Repository.Interfaces
{
    public interface IGlobalConfigRepository
    {
        GlobalConfig Get();
        void Update(GlobalConfig config);
    }
}

