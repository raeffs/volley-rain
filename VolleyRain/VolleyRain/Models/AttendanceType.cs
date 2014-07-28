using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public enum AttendanceType
    {
        [Display(Name = "?")]
        Unknown = 0,

        [Display(Name = "Anwesend")]
        Attending = 1,

        [Display(Name = "Abwesend")]
        Absent = 2,

        [Display(Name = "Krank")]
        Sickness = 3
    }
}