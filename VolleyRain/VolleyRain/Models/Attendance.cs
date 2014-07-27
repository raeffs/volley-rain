using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Attendance
    {
        public int ID { get; set; }

        public AttendanceType Type { get; set; }

        public virtual Event Event { get; set; }

        public virtual User User { get; set; }
    }
}