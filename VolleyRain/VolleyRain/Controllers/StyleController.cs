using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VolleyRain.Controllers
{
    public class StyleController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.AttendanceTypes = Context.AttendanceTypes.ToList();
            ViewBag.EventTypes = Context.EventTypes.ToList();

            return PartialView();
        }
    }
}