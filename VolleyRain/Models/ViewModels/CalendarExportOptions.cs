using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class CalendarExportOption
    {
        public int ID { get; set; }

        [Display(Name = "Anlass")]
        public EventType EventType { get; set; }

        [Display(Name = "Exportieren?")]
        public bool Export { get; set; }
    }
}