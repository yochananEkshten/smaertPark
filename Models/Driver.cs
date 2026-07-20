using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace smartPark.Models
{
 
    [Table("Driver")]
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(45)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(45)]
        public string LastName { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(20)]
        public string LicensePlate { get; set; }  // ייחודי לכל נהג

        public int RecordStatus { get; set; } = 1;  // 1 = פעיל, 2 = מחוק

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property – כל הכניסות של הנהג הזה
        public ICollection<VehicleEntry> VehicleEntries { get; set; }
       
    }

}
