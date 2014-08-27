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
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Index([TeamIdentifier] int? teamID, int? page, int? pageSize)
        {
            if (page.HasValue && page.Value == 0) return RedirectToAction("Archive");

            ViewBag.AttendanceTypes = Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList());
            ViewBag.EventTypes = Cache.GetEventTypes(() => Context.EventTypes.ToList());

            var pagination = new Pagination(pageSize ?? 10, Context.Events.Count(e => e.Team.ID == teamID.Value && e.Start >= DateTime.Today), page, true);
            ViewBag.Pagination = pagination;

            var events = Context.Events
                .Where(e => e.Team.ID == teamID.Value && e.Start >= DateTime.Today)
                .OrderBy(e => e.Start)
                .Skip(pagination.ItemsToSkip)
                .Take(pagination.PageSize)
                .Select(e => new EventSummary { ID = e.ID, Start = e.Start, TypeID = e.Type.ID, Name = e.Name })
                .ToList();
            var users = Context.Teams
                .Where(t => t.ID == teamID)
                .SelectMany(t => t.Members.Select(m => m.User))
                .Distinct()
                .Select(u => new UserSummary { ID = u.ID, Name = u.Name, Surname = u.Surname, IsCoach = u.Teams.Any(t => t.IsCoach) })
                .OrderByDescending(u => u.IsCoach)
                .ThenBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .ToList();

            var eventIDs = events.Select(e => e.ID).ToList();
            var userIDs = users.Select(u => u.ID).ToList();

            var attendances = Context.Attendances
                .Where(a => eventIDs.Contains(a.Event.ID) && userIDs.Contains(a.User.ID))
                .Select(a => new AttendanceSummary
                {
                    EventID = a.Event.ID,
                    UserID = a.User.ID,
                    TypeID = a.Type.ID,
                    TypeName = a.Type.Name,
                    TypeShortName = a.Type.ShortName,
                    RepresentsAttendance = a.Type.RepresentsAttendance,
                    Comment = a.Comment,
                })
                .ToList();

            var model = new AttendanceMatrix(events, users, attendances);

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Archive([TeamIdentifier] int? teamID, int? page, int? pageSize)
        {
            if (page.HasValue && page.Value == 0) return RedirectToAction("Index");

            ViewBag.AttendanceTypes = Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList());
            ViewBag.EventTypes = Cache.GetEventTypes(() => Context.EventTypes.ToList());

            var pagination = new ReversePagination(pageSize ?? 10, Context.Events.Count(e => e.Team.ID == teamID.Value && e.Start < DateTime.Today), page, true);
            ViewBag.Pagination = pagination;

            var events = Context.Events
                .Where(e => e.Team.ID == teamID.Value && e.Start < DateTime.Today)
                .OrderByDescending(e => e.Start)
                .Skip(pagination.ItemsToSkip)
                .Take(pagination.PageSize)
                .OrderBy(e => e.Start)
                .Select(e => new EventSummary { ID = e.ID, Start = e.Start, TypeID = e.Type.ID, Name = e.Name })
                .ToList();
            var users = Context.Teams
                .Where(t => t.ID == teamID)
                .SelectMany(t => t.Members.Select(m => m.User))
                .Distinct()
                .Select(u => new UserSummary { ID = u.ID, Name = u.Name, Surname = u.Surname, IsCoach = u.Teams.Any(t => t.IsCoach) })
                .OrderByDescending(u => u.IsCoach)
                .ThenBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .ToList();

            var eventIDs = events.Select(e => e.ID).ToList();
            var userIDs = users.Select(u => u.ID).ToList();

            var attendances = Context.Attendances
                .Where(a => eventIDs.Contains(a.Event.ID) && userIDs.Contains(a.User.ID))
                .Select(a => new AttendanceSummary
                {
                    EventID = a.Event.ID,
                    UserID = a.User.ID,
                    TypeID = a.Type.ID,
                    TypeName = a.Type.Name,
                    TypeShortName = a.Type.ShortName,
                    RepresentsAttendance = a.Type.RepresentsAttendance,
                    Comment = a.Comment,
                })
                .ToList();

            var model = new AttendanceMatrix(events, users, attendances);

            return View("Index", model);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Edit()
        {
            var noSelection = new AttendanceType { ID = 0, Name = string.Empty };
            var attendanceTypes = Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList()).Where(t => t.IsUserSelectable).ToList();
            attendanceTypes.Add(noSelection);
            ViewBag.AttendanceTypes = attendanceTypes.OrderBy(t => t.ID).ToList();

            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var teamIDs = Context.Teams.Where(t => t.Season.ID == season.ID && Session.Teams.Contains(t.ID)).Select(t => t.ID).ToList();

            if (teamIDs.Count == 0) return RedirectToAction("NoTeam", "Team");

            var tomorrow = DateTime.Today.AddDays(1);
            var model = Context.Events
                .Where(e => teamIDs.Contains(e.Team.ID) && e.Start >= tomorrow)
                .OrderBy(e => e.Start)
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
        public ActionResult Edit(IList<AttendanceSelection> model)
        {
            var attendanceTypes = Context.AttendanceTypes.Where(t => t.IsUserSelectable).ToList();

            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var teamIDs = Context.Teams.Where(t => t.Season.ID == season.ID).Select(t => t.ID).ToList();

            var tomorrow = DateTime.Today.AddDays(1);
            var eventIDs = Context.Events
                .Where(e => teamIDs.Contains(e.Team.ID) && e.Start >= tomorrow)
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

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Types()
        {
            return View(Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList()));
        }

        [HttpPost]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Types(IList<AttendanceType> model)
        {
            return RedirectToAction("Types");
        }
    }
}