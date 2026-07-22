using Microsoft.AspNetCore.Mvc;
using smartPark.Models;
using smartPark.Services.Interfaces;

namespace smartPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpotController : ControllerBase
    {
        private readonly IParkingSpotService _parkingSpotService;
        private readonly ILogger<ParkingSpotController> _logger;

        public ParkingSpotController(IParkingSpotService parkingSpotService, ILogger<ParkingSpotController> logger)
        {
            _parkingSpotService = parkingSpotService;
            _logger = logger;
        }

        // GET api/ParkingSpot
        [HttpGet]
        public ActionResult<List<ParkingSpot>> GetAll()
        {
            return Ok(_parkingSpotService.GetAll());
        }

        // GET api/ParkingSpot/available
        [HttpGet("available")]
        public ActionResult<List<ParkingSpot>> GetAvailable()
        {
            return Ok(_parkingSpotService.GetAvailable());
        }

        // GET api/ParkingSpot/floor/2
        [HttpGet("floor/{floor}")]
        public ActionResult<List<ParkingSpot>> GetByFloor(int floor)
        {
            return Ok(_parkingSpotService.GetByFloor(floor));
        }

        // GET api/ParkingSpot/5
        [HttpGet("{id}")]
        public ActionResult<ParkingSpot> GetById(int id)
        {
            var spot = _parkingSpotService.GetById(id);
            if (spot == null) return NotFound();
            return Ok(spot);
        }
    }
}
