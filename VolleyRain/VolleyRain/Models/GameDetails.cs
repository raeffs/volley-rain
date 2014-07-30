using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class GameDetails
    {
        public int ID { get; set; }

        [Required]
        public virtual Event Event { get; set; }

        [Required]
        public int ExternalID { get; set; }

        [Required]
        public string HomeTeam { get; set; }

        [Required]
        public string GuestTeam { get; set; }

        [Required]
        public string Hall { get; set; }
    }
}