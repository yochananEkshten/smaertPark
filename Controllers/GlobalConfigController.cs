using Microsoft.AspNetCore.Mvc;
using smartPark.Models;
using smartPark.Services.Interfaces;

namespace smartPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalConfigController : ControllerBase
    {
        private readonly IGlobalConfigService _globalConfigService;
        private readonly ILogger<GlobalConfigController> _logger;

        public GlobalConfigController(IGlobalConfigService globalConfigService, ILogger<GlobalConfigController> logger)
        {
            _globalConfigService = globalConfigService;
            _logger = logger;
        }

        // GET api/GlobalConfig
        [HttpGet]
        public ActionResult<GlobalConfig> Get()
        {
            var config = _globalConfigService.Get();
            if (config == null) return NotFound();
            return Ok(config);
        }

        // PUT api/GlobalConfig
        [HttpPut]
        public ActionResult Update([FromBody] GlobalConfig config)
        {
            _globalConfigService.Update(config);
            return Ok();
        }
    }
}
