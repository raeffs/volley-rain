using System.ComponentModel.DataAnnotations;

namespace VolleyRain.Models
{
    public class AttendanceType
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Bezeichnung")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Kürzel")]
        public string ShortName { get; set; }

        [Required]
        [Display(Name = "Entspricht Anwesenheit?")]
        public bool RepresentsAttendance { get; set; }

        [Required]
        [Display(Name = "Farbe")]
        public string ColorCode { get; set; }

        [Required]
        [Display(Name = "Durch Benutzer selektierbar?")]
        public bool IsUserSelectable { get; set; }
    }
}