using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class AccountSummary
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public bool IsSelf { get; set; }
    }
}