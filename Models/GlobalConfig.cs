using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace smaertPark.Models
{
    
    [Table("GlobalConfig")]
    public class GlobalConfig
    {
        [Key]
        public int Id { get; set; }
        // פרטי החניון
        [MaxLength(100)]
        public string ParkingLotName { get; set; }       // שם החניון

        [MaxLength(200)]
        public string Address { get; set; }               // כתובת

        [MaxLength(150)]
        public string ScreenTitle { get; set; }           // כותרת במסך הכניסה

        // קיבולת
        public int TotalSpots { get; set; }               // סך מקומות חניה
        public int Floors { get; set; }                   // מספר קומות

        // שעות פעילות
        public TimeSpan OpeningTime { get; set; }         // שעת פתיחה
        public TimeSpan ClosingTime { get; set; }         // שעת סגירה

        // תעריפים
        public decimal PricePerHourBusiness { get; set; } // מחיר לשעה בשעות עסקים
        public decimal PricePerHourRegular { get; set; }  // מחיר לשעה בשעות רגילות
        public TimeSpan BusinessHoursStart { get; set; }  // תחילת שעות עסקים
        public TimeSpan BusinessHoursEnd { get; set; }    // סוף שעות עסקים

        // הגדרות נוספות
        [MaxLength(10)]
        public string Currency { get; set; }              // מטבע (ILS)
        public int MaxParkingHours { get; set; }          // מקסימום שעות חניה
        public bool LicensePlateRecognitionEnabled { get; set; } // זיהוי לוחית רישוי פעיל
    }

}
