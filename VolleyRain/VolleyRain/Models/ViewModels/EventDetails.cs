using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class EventDetails
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Display(Name = "Ort")]
        public string Location { get; set; }

        [Display(Name = "Typ")]
        public string TypeName { get; set; }

        [Display(Name = "Begin")]
        public DateTime Start { get; set; }

        [Display(Name = "Ende")]
        public DateTime End { get; set; }
    }
}