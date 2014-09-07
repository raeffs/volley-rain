using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class EffectiveAttendanceSelection
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string UserSurname { get; set; }

        [Display(Name = "Zustand")]
        public AttendanceType AttendanceType { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }
    }
}