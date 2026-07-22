using smartPark.Services.Interfaces;
using smartPark.Models;
using smartPark.Repository.Interfaces;

namespace smartPark.Services
{
    public class GlobalConfigService : IGlobalConfigService
    {
        private readonly IGlobalConfigRepository _globalConfigRepository;
        private readonly ILogger<GlobalConfigService> _logger;

        public GlobalConfigService(IGlobalConfigRepository globalConfigRepository, ILogger<GlobalConfigService> logger)
        {
            _globalConfigRepository = globalConfigRepository;
            _logger = logger;
        }

        public GlobalConfig Get()
        {
            return _globalConfigRepository.Get();
        }

        public void Update(GlobalConfig config)
        {
            _globalConfigRepository.Update(config);
        }
    }
}
