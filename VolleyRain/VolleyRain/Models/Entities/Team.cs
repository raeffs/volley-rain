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

        [Required]
        public int ExternalID { get; set; }

        [Required]
        public int ExternalGroupID { get; set; }

        [Required]
        public virtual Season Season { get; set; }

        public ICollection<TeamMembership> Members { get; set; }

        public Team()
        {
            Members = new List<TeamMembership>();
        }
    }
}