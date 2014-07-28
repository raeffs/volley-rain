using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Event
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public virtual EventType Type { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public virtual List<Attendance> Attendances { get; set; }

        public Event()
        {
            Attendances = new List<Attendance>();
        }
    }
}