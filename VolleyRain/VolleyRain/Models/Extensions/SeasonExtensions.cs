using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public static class SeasonExtensions
    {
        public static Season GetActualSeason(this IEnumerable<Season> seasons)
        {
            return seasons
                .Select(s => new { ID = s.ID, Period = new Itenso.TimePeriod.TimeRange(s.Start, s.End) })
                .Where(a => a.Period.HasInside(DateTime.Today))
                .Select(a => seasons.Single(s => s.ID == a.ID))
                .SingleOrDefault()
                ?? seasons
                .OrderByDescending(s => s.Start)
                .First();
        }
    }
}