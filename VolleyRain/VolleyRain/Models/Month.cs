using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Month
    {
        [DisplayFormat(DataFormatString = "{0:MMMM yyyy}")]
        public DateTime Date { get; set; }

        public IEnumerable<Day> Days { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMMM yyyy}")]
        public DateTime NextMonth { get { return Date.AddMonths(1); } }

        [DisplayFormat(DataFormatString = "{0:MMMM yyyy}")]
        public DateTime PreviousMonth { get { return Date.AddMonths(-1); } }

        public Month()
        {
            Days = new Day[] { };
        }
    }
}