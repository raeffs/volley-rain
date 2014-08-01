using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class AttendanceSummary
    {
        public int EventID { get; set; }
        public int UserID { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public bool RepresentsAttendance { get; set; }
        public string Comment { get; set; }
        public bool HasComment { get { return !string.IsNullOrWhiteSpace(Comment); } }
    }
}