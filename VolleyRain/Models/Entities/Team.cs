using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Team
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "RVI Team ID")]
        public int? ExternalID { get; set; }

        [Display(Name = "RVI Gruppen ID")]
        public int? ExternalGroupID { get; set; }

        [Required]
        [Display(Name = "Saison")]
        public virtual Season Season { get; set; }

        public virtual ICollection<TeamMembership> Members { get; set; }

        public virtual ICollection<Ranking> Rankings { get; set; }

        public Team()
        {
            Members = new List<TeamMembership>();
            Rankings = new List<Ranking>();
        }
    }
}