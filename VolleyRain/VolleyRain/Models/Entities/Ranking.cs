using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Ranking
    {
        public int ID { get; set; }

        [Display(Name = "Rang")]
        public int Rank { get; set; }

        [Display(Name = "Team")]
        public string Team { get; set; }

        [Display(Name = "Spiele")]
        public int NumberOfGames { get; set; }

        public int SetsWon { get; set; }

        public int SetsLost { get; set; }

        [NotMapped]
        [Display(Name = "Sätze")]
        public string Sets { get { return string.Format("{0} : {1}", SetsWon, SetsLost); } }

        [Display(Name = "SQ")]
        public decimal SetQuotient { get; set; }

        public int BallsWon { get; set; }

        public int BallsLost { get; set; }

        [NotMapped]
        [Display(Name = "Bälle")]
        public string Balls { get { return string.Format("{0} : {1}", BallsWon, BallsLost); } }

        [Display(Name = "BQ")]
        public decimal BallQuotient { get; set; }

        [Display(Name = "Punkte")]
        public int Points { get; set; }
    }
}