using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace smaertPark.Models
{


   
    [Table("VehicleEntry")]
    public class VehicleEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string PlateNumber { get; set; }

        public DateTime EntryTime { get; set; } = DateTime.UtcNow;

        public DateTime? ExitTime { get; set; }

        public bool AutoDetected { get; set; }

        public int? SpotId { get; set; }  // איזה מקום חניה הוקצה לרכב

        [Required]
        public VehicleStatus Status { get; set; } = VehicleStatus.Inside;  // בחניון / יצא

        // Navigation Property
        [ForeignKey("SpotId")]
        public ParkingSpot Spot { get; set; }
        public int? DriverId { get; set; }

        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }
    }

}
