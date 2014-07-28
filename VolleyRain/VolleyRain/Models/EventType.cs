using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class EventType
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string ColorCode { get; set; }

        public string GetCssClass()
        {
            return string.Format("event-type-{0}", ID);
        }
    }
}