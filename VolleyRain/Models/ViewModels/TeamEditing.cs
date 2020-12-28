using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class TeamEditing
    {
        public int ID { get; set; }

        public string Name { get; set; }

        [Display(Name = "RVI Team ID")]
        public int? ExternalID { get; set; }

        [Display(Name = "RVI Gruppen ID")]
        public int? ExternalGroupID { get; set; }

        [Display(Name = "Saison")]
        public string Season { get; set; }
    }
}