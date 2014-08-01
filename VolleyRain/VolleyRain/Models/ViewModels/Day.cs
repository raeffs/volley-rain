using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Day
    {
        public DateTime Date { get; private set; }

        public int NumberOfEvents { get; set; }

        public bool IsInsideMonth { get; set; }

        public Itenso.TimePeriod.ITimePeriod CalendarViewPeriod { get; private set; }

        public List<EventSummary> Events { get; set; }

        public Day(int year, int month, int day)
        {
            var dayBase = new Itenso.TimePeriod.Day(year, month, day);
            Date = dayBase.FirstDayStart;
            CalendarViewPeriod = dayBase;
            Events = new List<EventSummary>();
        }
    }
}