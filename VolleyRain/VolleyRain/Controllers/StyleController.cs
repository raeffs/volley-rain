using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class StyleController : BaseController
    {
        private const string KEY_ATTENDANCE_TYPES = "AttendanceTypes";
        private const string KEY_EVENT_TYPES = "EventTypes";

        public ActionResult Index()
        {
            ViewBag.AttendanceTypes = GetAttendanceTypes();
            ViewBag.EventTypes = GetEventTypes();

            return PartialView();
        }

        private ICollection<AttendanceType> GetAttendanceTypes()
        {
            if (HttpContext.Cache[KEY_ATTENDANCE_TYPES] != null) return (ICollection<AttendanceType>)HttpContext.Cache[KEY_ATTENDANCE_TYPES];

            ICollection<AttendanceType> types = Context.AttendanceTypes.ToList();
            HttpContext.Cache.Insert(KEY_ATTENDANCE_TYPES, types, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            return types;
        }

        private ICollection<EventType> GetEventTypes()
        {
            if (HttpContext.Cache[KEY_EVENT_TYPES] != null) return (ICollection<EventType>)HttpContext.Cache[KEY_EVENT_TYPES];

            ICollection<EventType> types = Context.EventTypes.ToList();
            HttpContext.Cache.Insert(KEY_EVENT_TYPES, types, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            return types;
        }
    }
}