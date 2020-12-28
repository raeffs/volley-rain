using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class AttendanceSelection
    {
        public int ID { get; set; }

        public DateTime Start { private get; set; }

        [Display(Name = "Datum")]
        [DisplayFormat(DataFormatString = "{0:ddd dd.MM.yyyy}")]
        public DateTime StartDate { get { return Start; } }

        [Display(Name = "Zeit")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime StartTime { get { return Start; } }

        [Display(Name = "Anlass")]
        public EventType EventType { get; set; }

        [Display(Name = "Zustand")]
        public AttendanceType AttendanceType { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }
    }
}