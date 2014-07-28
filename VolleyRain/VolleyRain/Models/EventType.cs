using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    //public enum EventType
    //{
    //    [Display(Name = "Ligaspiel")]
    //    Match = 0,

    //    [Display(Name = "Freundschaftsspiel")]
    //    FriendlyMatch = 1,

    //    [Display(Name = "Training")]
    //    Training = 2,
    //}

    public class EventType
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string ColorCode { get; set; }
    }
}