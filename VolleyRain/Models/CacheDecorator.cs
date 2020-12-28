using System;
using System.Collections.Generic;
using System.Web.Caching;

namespace VolleyRain.Models
{
    public class CacheDecorator
    {
        private readonly Cache _context;

        public CacheDecorator(Cache context)
        {
            _context = context;
        }

        public Season GetSeason(Func<Season> initializer)
        {
            return GetValue("Season", initializer);
        }

        public Season GetSeasonWithTeams(Func<Season> initializer)
        {
            return GetValue("SeasonWithTeams", initializer);
        }

        public ICollection<AttendanceType> GetAttendanceTypes(Func<ICollection<AttendanceType>> initializer, bool forceUpdate = false)
        {
            return GetValue("AttendanceTypes", initializer, forceUpdate);
        }

        public ICollection<EventType> GetEventTypes(Func<ICollection<EventType>> initializer, bool forceUpdate = false)
        {
            return GetValue("EventTypes", initializer, forceUpdate);
        }

        private T GetValue<T>(string key, Func<T> initializer, bool forceUpdate = false)
        {
            if (_context[key] != null && !forceUpdate) return (T)_context[key];
            T value = initializer();
            _context.Insert(key, value, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            return value;
        }
    }
}