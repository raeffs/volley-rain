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
        public AttendanceController()
        {
            ViewBag.PageSize = 10;
        }

        [Authorize(Roles = "User")]
        public ActionResult Index(int? teamID, int? page)
        {
            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var teamIDs = Context.Teams.Where(t => t.Season.ID == season.ID).Select(t => t.ID).ToList();

            var pagination = new Pagination(
                10,
                Context.Events.Count(e => teamIDs.Contains(e.Team.ID)),
                page);
            if (!page.HasValue) pagination.JumpToItem(Context.Events.Count(e => teamIDs.Contains(e.Team.ID) && e.Start < DateTime.Today) + 1);
            ViewBag.Pagination = pagination;

            var events = Context.Events
                .Where(e => teamIDs.Contains(e.Team.ID))
                .OrderBy(e => e.Start)
                .Skip(pagination.ItemsToSkip)
                .Take(pagination.PageSize)
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
            ViewBag.AttendanceTypes = new SelectList(Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList()), "ID", "Name");

            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var teamIDs = Context.Teams.Where(t => t.Season.ID == season.ID).Select(t => t.ID).ToList();

            var model = Context.Events
                .Where(e => teamIDs.Contains(e.Team.ID))
                .Select(e => new AttendanceSelection
                {
                    ID = e.ID,
                    Start = e.Start,
                    EventType = e.Type,
                })
                .ToList();

            var ids = model.Select(m => m.ID).ToList();

            foreach (var attendance in Context.Attendances.Where(a => a.User.ID == Session.UserID && ids.Contains(a.Event.ID)))
            {
                model.Single(m => m.ID == attendance.Event.ID).AttendanceType = attendance.Type;
            }

            return View(model);
        }
    }
}