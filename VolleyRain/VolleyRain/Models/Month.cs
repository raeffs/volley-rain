using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Month
    {
        public DateTime Date { get; set; }
        public IEnumerable<Day> Days { get; set; }

        public Month()
        {
            Days = new Day[] { };
        }
    }
}