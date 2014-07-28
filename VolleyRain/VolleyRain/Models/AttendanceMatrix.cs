using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class AttendanceMatrix
    {
        private readonly List<Event> _events;
        private readonly List<User> _users;
        private readonly List<Attendance> _attendances;

        public AttendanceMatrix(List<Event> events, List<User> users, List<Attendance> attendances)
        {
            _events = events;
            _users = users;
            _attendances = attendances;
        }

        public List<Event> Events { get { return _events; } }

        public List<User> Users { get { return _users; } }

        public AttendanceType GetAttendanceFor(User user, Event @event)
        {
            return _attendances.Where(a => a.User == user && a.Event == @event).Select(a => a.Type).SingleOrDefault();
        }

        public int GetAttendeesFor(Event @event)
        {
            return _attendances.Count(a => a.Event == @event && a.Type.RepresentsAttendance);
        }
    }
}