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

        public User SelectedUser { get; set; }

        public AttendanceType this[Event index]
        {
            get
            {
                if (SelectedUser == null) return AttendanceType.Unknown;

                var attendance = _attendances.SingleOrDefault(a => a.User == SelectedUser && a.Event == index);
                return attendance != null ? attendance.Type : AttendanceType.Unknown;
            }
        }
    }
}