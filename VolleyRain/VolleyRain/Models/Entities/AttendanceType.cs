using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        public bool RepresentsAttendance { get; set; }

        [Required]
        public string ColorCode { get; set; }

        [Required]
        public bool IsUserSelectable { get; set; }
    }
}