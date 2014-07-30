using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ICollection<AttendanceType> GetAttendanceTypes(Func<ICollection<AttendanceType>> initializer)
        {
            return GetValue("AttendanceTypes", initializer);
        }

        public ICollection<EventType> GetEventTypes(Func<ICollection<EventType>> initializer)
        {
            return GetValue("EventTypes", initializer);
        }

        private T GetValue<T>(string key, Func<T> initializer)
        {
            if (_context[key] != null) return (T)_context[key];
            T value = initializer();
            _context.Insert(key, value, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            return value;
        }
    }
}