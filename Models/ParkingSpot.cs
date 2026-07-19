using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace smaertPark.Models
{
    [Table("ParkingSpot")]
    public class ParkingSpot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string SpotNumber { get; set; }  // A1, B3 וכו'

        [Required]
        public int Floor { get; set; }  // קומה

        [Required]
        public SpotStatus StatusId { get; set; }  // פנוי / תפוס / מושבת

        public VehicleEntry? Vehicle { get; set; } // איזה רכב חונה כרגע (null אם פנוי)

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

      
        
    }
}