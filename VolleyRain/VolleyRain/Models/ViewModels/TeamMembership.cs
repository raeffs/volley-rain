using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class TeamMembership
    {
        public int UserID { get; set; }

        [Display(Name = "Vorname")]
        public string Name { get; set; }

        [Display(Name = "Nachname")]
        public string Surname { get; set; }

        [Display(Name = "Mitglied?")]
        public bool IsMemberOfTeam { get; set; }

        [Display(Name = "Administrator?")]
        public bool IsAdminOfTeam { get; set; }

        public bool IsSelf { get; set; }
    }
}