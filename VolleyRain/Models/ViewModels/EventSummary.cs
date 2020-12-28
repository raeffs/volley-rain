using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class EventSummary
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}