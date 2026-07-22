using Microsoft.AspNetCore.Mvc;
using smartPark.Models;
using smartPark.Services.Interfaces;

namespace smartPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ILogger<DriverController> _logger;

        public DriverController(IDriverService driverService, ILogger<DriverController> logger)
        {
            _driverService = driverService;
            _logger = logger;
        }

        // GET api/Driver
        [HttpGet]
        public ActionResult<List<Driver>> GetAll()
        {
            return Ok(_driverService.GetAll());
        }

        // GET api/Driver/5
        [HttpGet("{id}")]
        public ActionResult<Driver> GetById(int id)
        {
            var driver = _driverService.GetById(id);
            if (driver == null) return NotFound();
            return Ok(driver);
        }

        // GET api/Driver/plate/12-345-67
        [HttpGet("plate/{licensePlate}")]
        public ActionResult<Driver> GetByLicensePlate(string licensePlate)
        {
            var driver = _driverService.GetByLicensePlate(licensePlate);
            if (driver == null) return NotFound();
            return Ok(driver);
        }

        // POST api/Driver
        [HttpPost]
        public ActionResult<int> Add([FromBody] Driver driver)
        {
            int id = _driverService.Add(driver);
            return Ok(id);
        }

        // PUT api/Driver
        [HttpPut]
        public ActionResult Update([FromBody] Driver driver)
        {
            _driverService.Update(driver);
            return Ok();
        }

        // DELETE api/Driver/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _driverService.Delete(id);
            return Ok();
        }
    }
}
