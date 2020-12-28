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

        [Required(AllowEmptyStrings = true)]
        public string Hall { get; set; }

        [Required]
        public bool IsCommited { get; set; }

        [Required]
        public int SetsWonHome { get; set; }

        [Required]
        public int SetsWonGuest { get; set; }

        [Required]
        public int Set1PointsHome { get; set; }

        [Required]
        public int Set1PointsGuest { get; set; }

        [Required]
        public int Set2PointsHome { get; set; }

        [Required]
        public int Set2PointsGuest { get; set; }

        [Required]
        public int Set3PointsHome { get; set; }

        [Required]
        public int Set3PointsGuest { get; set; }

        [Required]
        public int Set4PointsHome { get; set; }

        [Required]
        public int Set4PointsGuest { get; set; }

        [Required]
        public int Set5PointsHome { get; set; }

        [Required]
        public int Set5PointsGuest { get; set; }
    }
}