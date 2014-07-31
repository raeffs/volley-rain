using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Attendance
    {
        public int ID { get; set; }

        [Required]
        public virtual AttendanceType Type { get; set; }

        [Required]
        public virtual Event Event { get; set; }

        [Required]
        public virtual User User { get; set; }

        public string Comment { get; set; }
    }
}