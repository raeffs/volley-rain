using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class TeamMembershipSelection
    {
        public int UserID { get; set; }

        [Display(Name = "Vorname")]
        public string Name { get; set; }

        [Display(Name = "Nachname")]
        public string Surname { get; set; }

        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Display(Name = "Mitglied?", ShortName = "M?")]
        public bool IsMemberOfTeam { get; set; }

        [Display(Name = "Trainer?", ShortName = "T?")]
        public bool IsCoachOfTeam { get; set; }

        [Display(Name = "Administrator?", ShortName = "A?")]
        public bool IsAdminOfTeam { get; set; }

        public bool IsSelf { get; set; }
    }
}