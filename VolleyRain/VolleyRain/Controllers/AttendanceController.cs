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
        [Authorize(Roles = "User")]
        public ActionResult Index(int? teamID)
        {
            var events = Context.Events
                .Where(e => e.Start >= DateTime.Today && (e.Team == null || Session.Teams.Contains(e.Team.ID)))
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

        [Authorize(Roles = "User")]
        public ActionResult Edit()
        {
            return View();
        }
    }
}