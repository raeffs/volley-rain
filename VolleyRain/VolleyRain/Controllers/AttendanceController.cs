using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.DataAccess;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class AttendanceController : BaseController
    {
        public ActionResult Index()
        {
            var events = Context.Events
                .Where(e => e.Start >= DateTime.Today)
                .Take(10)
                .ToList();
            var users = Context.Users
                .ToList();
            var attendances = events
                .SelectMany(e => e.Attendances)
                .ToList();

            var model = new AttendanceMatrix(events, users, attendances);

            return View(model);
        }
    }
}