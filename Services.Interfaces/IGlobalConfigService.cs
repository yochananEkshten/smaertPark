
using smartPark.Models;

namespace smartPark.Services.Interfaces
{
    public interface IGlobalConfigService
    {
        GlobalConfig Get();
        void Update(GlobalConfig config);
    }
}

