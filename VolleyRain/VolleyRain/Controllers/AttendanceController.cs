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
        public ActionResult Index(int? teamID, int? page)
        {
            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var teamIDs = Context.Teams.Where(t => t.Season.ID == season.ID).Select(t => t.ID).ToList();
            var events = Context.Events
                .Where(e => e.Start >= DateTime.Today && teamIDs.Contains(e.Team.ID))
                .Take(10)
                .ToList();
            var users = Context.Teams
                .Where(t => teamIDs.Contains(t.ID))
                .SelectMany(t => t.Members)
                .Distinct()
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