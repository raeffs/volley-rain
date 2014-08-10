using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class EventCreation
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Display(Name = "Wo")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Was")]
        public EventType Type { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Datum (von)")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Datum (bis)")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Ganztägig")]
        public bool FullTime { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Zeit (von)")]
        public DateTime? StartTime { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Zeit (bis)")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "Wöchentlich wiederholen")]
        public bool RecurrsWeekly { get; set; }

        [Display(Name = "Anzahl Wiederholungen")]
        [Range(1, 99)]
        public int? NumberOfRecurrences { get; set; }
    }
}