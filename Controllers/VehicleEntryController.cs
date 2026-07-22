using Microsoft.AspNetCore.Mvc;
using smartPark.Models;
using smartPark.Services.Interfaces;

namespace smartPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleEntryController : ControllerBase
    {
        private readonly IVehicleEntryService _vehicleEntryService;
        private readonly ILogger<VehicleEntryController> _logger;

        public VehicleEntryController(IVehicleEntryService vehicleEntryService, ILogger<VehicleEntryController> logger)
        {
            _vehicleEntryService = vehicleEntryService;
            _logger = logger;
        }

        // GET api/VehicleEntry
        [HttpGet]
        public ActionResult<List<VehicleEntry>> GetAll()
        {
            return Ok(_vehicleEntryService.GetAll());
        }

        // GET api/VehicleEntry/active
        [HttpGet("active")]
        public ActionResult<List<VehicleEntry>> GetActive()
        {
            return Ok(_vehicleEntryService.GetActive());
        }

        // GET api/VehicleEntry/5
        [HttpGet("{id}")]
        public ActionResult<VehicleEntry> GetById(int id)
        {
            var vehicle = _vehicleEntryService.GetById(id);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }

        // GET api/VehicleEntry/plate/12-345-67
        [HttpGet("plate/{licensePlate}")]
        public ActionResult<VehicleEntry> GetActiveByLicensePlate(string licensePlate)
        {
            var vehicle = _vehicleEntryService.GetActiveByLicensePlate(licensePlate);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }

        // POST api/VehicleEntry/entry
        [HttpPost("entry")]
        public ActionResult<int> Entry([FromBody] VehicleEntryRequest request)
        {
            int id = _vehicleEntryService.VehicleEntry(request.LicensePlate, request.DriverId);
            return Ok(id);
        }

        // POST api/VehicleEntry/exit/5
        [HttpPost("exit/{id}")]
        public ActionResult<decimal> Exit(int id)
        {
            decimal payment = _vehicleEntryService.VehicleExit(id);
            return Ok(payment);
        }
    }

    // Request model לכניסת רכב
    public class VehicleEntryRequest
    {
        public string LicensePlate { get; set; }
        public int? DriverId { get; set; }
    }
}
