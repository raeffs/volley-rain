using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var noSelection = new AttendanceType { ID = 0, Name = string.Empty };
            var attendanceTypes = Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList()).ToList();
            attendanceTypes.Add(noSelection);
            ViewBag.AttendanceTypes = attendanceTypes.OrderBy(t => t.ID).ToList();

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
                var item = model.Single(m => m.ID == attendance.Event.ID);
                item.AttendanceType = attendance.Type;
                item.Comment = attendance.Comment;
            }
            foreach (var item in model)
            {
                if (item.AttendanceType == null) item.AttendanceType = noSelection;
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult Edit([Bind] IList<AttendanceSelection> model)
        {
            var attendanceTypes = Context.AttendanceTypes.ToList();

            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var teamIDs = Context.Teams.Where(t => t.Season.ID == season.ID).Select(t => t.ID).ToList();

            var eventIDs = Context.Events
                .Where(e => teamIDs.Contains(e.Team.ID))
                .Select(m => m.ID)
                .ToList();
            var attendances = Context.Attendances.Include(a => a.User).Where(a => a.User.ID == Session.UserID && eventIDs.Contains(a.Event.ID)).ToList();
            var toRemove = new List<Attendance>();

            foreach (var item in model.Where(i => eventIDs.Contains(i.ID)))
            {
                var type = attendanceTypes.SingleOrDefault(t => t.ID == item.AttendanceType.ID);
                if (attendances.Any(a => a.Event.ID == item.ID))
                {
                    if (type == null)
                    {
                        toRemove.Add(attendances.Single(a => a.Event.ID == item.ID));
                    }
                    else
                    {
                        var attendance = attendances.Single(a => a.Event.ID == item.ID);
                        attendance.Type = type;
                        attendance.Comment = item.Comment;
                    }
                }
                else
                {
                    if (type == null) continue;

                    Context.Attendances.Add(new Attendance
                    {
                        User = Context.Users.Single(u => u.ID == Session.UserID),
                        Event = Context.Events.Single(e => e.ID == item.ID),
                        Type = type,
                        Comment = item.Comment,
                    });
                }
            }
            Context.Attendances.RemoveRange(toRemove);
            Context.SaveChanges();

            return RedirectToAction("Edit");
        }
    }
}