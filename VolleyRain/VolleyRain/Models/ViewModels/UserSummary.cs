using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class UserSummary
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsCoach { get; set; }
    }
}