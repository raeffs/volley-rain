using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class SeasonSummary
    {
        public int ID { get; set; }
        
        public string Name { get; set; }

        public virtual ICollection<TeamSummary> Teams { get; set; }

        public SeasonSummary()
        {
            Teams = new List<TeamSummary>();
        }
    }
}