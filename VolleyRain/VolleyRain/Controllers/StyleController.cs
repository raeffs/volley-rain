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
        public ActionResult Render()
        {
            ViewBag.AttendanceTypes = Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList());
            ViewBag.EventTypes = Cache.GetEventTypes(() => Context.EventTypes.ToList());

            return PartialView();
        }
    }
}