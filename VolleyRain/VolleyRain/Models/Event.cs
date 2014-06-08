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

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }
    }
}