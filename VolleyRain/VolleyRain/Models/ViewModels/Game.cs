using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Game
    {
        public DateTime Start { private get; set; }

        [Display(Name = "Datum")]
        [DisplayFormat(DataFormatString = "{0:ddd, dd. MMMM}")]
        public DateTime StartDate { get { return Start; } }

        [Display(Name = "Zeit")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime StartTime { get { return Start; } }

        [Display(Name = "Heim")]
        public string HomeTeam { get; set; }

        [Display(Name = "Gast")]
        public string GuestTeam { get; set; }

        [Display(Name = "Halle")]
        public string Hall { get; set; }
    }
}