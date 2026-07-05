using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace smaertPark.Models
{


    public class VehicleEntry
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public bool AutoDetected { get; set; }
    }

}
