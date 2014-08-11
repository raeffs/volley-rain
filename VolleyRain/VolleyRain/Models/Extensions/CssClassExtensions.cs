using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public static class CssClassExtensions
    {
        public static string GetCssClass(this AttendanceType value)
        {
            return string.Format("attendance-type-{0}", value.ID);
        }

        public static string GetCssClass(this AttendanceSummary value)
        {
            return string.Format("attendance-type-{0}", value.TypeID);
        }

        public static string GetCssClass(this EventType value)
        {
            return string.Format("event-type-{0}", value.ID);
        }

        public static string GetCssClass(this EventSummary value)
        {
            return string.Format("event-type-{0}", value.TypeID);
        }

        public static string GetCssClass(this LogSummary value)
        {
            return string.Format("log-level-{0}", value.Level.ToLower());
        }
    }
}