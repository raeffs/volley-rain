using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class AttendanceMatrix
    {
        private readonly List<EventSummary> _events;
        private readonly List<UserSummary> _users;
        private readonly List<AttendanceSummary> _attendances;

        public AttendanceMatrix(List<EventSummary> events, List<UserSummary> users, List<AttendanceSummary> attendances)
        {
            _events = events;
            _users = users;
            _attendances = attendances;
        }

        public List<EventSummary> Events { get { return _events; } }

        public List<UserSummary> Users { get { return _users; } }

        public AttendanceSummary GetAttendanceFor(UserSummary user, EventSummary @event)
        {
            return _attendances.SingleOrDefault(a => a.UserID == user.ID && a.EventID == @event.ID);
        }

        public int GetAttendeesFor(EventSummary @event)
        {
            return _attendances.Count(a => a.EventID == @event.ID && a.RepresentsAttendance);
        }
    }
}