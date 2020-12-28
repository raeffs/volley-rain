using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class TeamMembership
    {
        [Required]
        public virtual int TeamID { get; set; }

        public virtual Team Team { get; set; }

        [Required]
        public virtual int UserID { get; set; }

        public virtual User User { get; set; }

        public bool IsCoach { get; set; }

        public bool IsTemporary { get; set; }

        public bool IsAdmin { get; set; }
    }
}