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

        public string Name { get; set; }

        public string ShortName { get; set; }

        public bool RepresentsAttendance { get; set; }

        public string ColorCode { get; set; }
    }
}