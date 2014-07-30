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
        public string Name { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public bool RepresentsAttendance { get; set; }

        [Required]
        public string ColorCode { get; set; }

        public string GetCssClass()
        {
            return string.Format("attendance-type-{0}", ID);
        }
    }
}