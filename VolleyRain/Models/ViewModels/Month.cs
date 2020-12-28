using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Month
    {
        public IList<Day> Days { get; private set; }

        [DisplayFormat(DataFormatString = "{0:MMMM yyyy}")]
        public DateTime Date { get; private set; }

        [DisplayFormat(DataFormatString = "{0:MMMM yyyy}")]
        public DateTime NextMonth { get; private set; }

        [DisplayFormat(DataFormatString = "{0:MMMM yyyy}")]
        public DateTime PreviousMonth { get; private set; }

        public Itenso.TimePeriod.ITimePeriod CalendarViewPeriod { get; private set; }

        public Month(int year, int month)
        {
            var monthBase = new Itenso.TimePeriod.Month(new DateTime(year, month, 1));

            Days = new List<Day>();
            Date = monthBase.FirstDayStart;
            NextMonth = monthBase.GetNextMonth().FirstDayStart;
            PreviousMonth = monthBase.GetPreviousMonth().FirstDayStart;

            CalendarViewPeriod = new Itenso.TimePeriod.CalendarTimeRange(
                monthBase.FirstDayStart.AddDays(-1 * ((int)monthBase.FirstDayStart.DayOfWeek + 6) % 7),
                monthBase.LastDayStart.AddDays((7 - (int)monthBase.LastDayStart.DayOfWeek) % 7 + 1));

            var day = CalendarViewPeriod.Start;
            while (CalendarViewPeriod.HasInside(day))
            {
                var model = new Day(day.Year, day.Month, day.Day);
                model.IsInsideMonth = monthBase.HasInside(day);
                Days.Add(model);
                day = day.AddDays(1);
            }
        }
    }
}