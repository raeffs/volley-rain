﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class AttendanceController : BaseController
    {
        private void SetTeam(int? teamID)
        {
            var team = Context.Teams
                .Include(t => t.Members)
                .Include(t => t.Season)
                .Single(t => t.ID == teamID);
            ViewBag.Team = team;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Index([TeamIdentifier] int? teamID, int? page, int? pageSize)
        {
            SetTeam(teamID);

            if (page.HasValue && page.Value == 0) return RedirectToAction("Archive", new { pageSize = pageSize });

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
                .Select(u => new UserSummary
                {
                    ID = u.ID,
                    Name = u.Name,
                    Surname = u.Surname,
                    IsCoach = u.Teams.Any(t => t.IsCoach),
                    IsTemporary = u.Teams.Any(t => t.IsTemporary),
                    NumberOfAttendings = Context.Attendances.Count(a => a.User.ID == u.ID && a.Event.Team.ID == teamID.Value && a.Event.Start < DateTime.Today && a.Type.RepresentsAttendance),
                    PossibleAttendings = Context.Events.Count(e => e.Start < DateTime.Today && e.Team.ID == teamID.Value),
                })
                .OrderByDescending(u => u.IsCoach)
                .ThenBy(u => u.IsTemporary)
                .ThenBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .ToList();
            var userIDs = users.Select(u => u.ID).ToList();
            var eventIDs = events.Select(e => e.ID).ToList();

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
            SetTeam(teamID);

            if (page.HasValue && page.Value == 0) return RedirectToAction("Index", new { pageSize = pageSize });

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
                .Select(u => new UserSummary
                {
                    ID = u.ID,
                    Name = u.Name,
                    Surname = u.Surname,
                    IsCoach = u.Teams.Any(t => t.IsCoach),
                    IsTemporary = u.Teams.Any(t => t.IsTemporary),
                    NumberOfAttendings = Context.Attendances.Count(a => a.User.ID == u.ID && a.Event.Team.ID == teamID.Value && a.Event.Start < DateTime.Today && a.Type.RepresentsAttendance),
                    PossibleAttendings = Context.Events.Count(e => e.Start < DateTime.Today && e.Team.ID == teamID.Value),
                })
                .OrderByDescending(u => u.IsCoach)
                .ThenBy(u => u.IsTemporary)
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
        public ActionResult Edit([TeamIdentifier] int? teamID)
        {
            SetTeam(teamID);

            var noSelection = new AttendanceType { ID = 0, Name = string.Empty };
            var attendanceTypes = Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList()).Where(t => t.IsUserSelectable).ToList();
            attendanceTypes.Add(noSelection);
            ViewBag.AttendanceTypes = attendanceTypes.OrderBy(t => t.ID).ToList();

            var tomorrow = DateTime.Today.AddDays(1);
            var model = Context.Events
                .Where(e => e.Team.ID == teamID.Value && e.Start >= tomorrow)
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
        public ActionResult Edit([TeamIdentifier] int? teamID, IList<AttendanceSelection> model)
        {
            SetTeam(teamID);

            var attendanceTypes = Context.AttendanceTypes.Where(t => t.IsUserSelectable).ToList();

            var tomorrow = DateTime.Today.AddDays(1);
            var eventIDs = Context.Events
                .Where(e => e.Team.ID == teamID.Value && e.Start >= tomorrow)
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

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult CreateType()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult CreateType(AttendanceTypeCreation model)
        {
            if (ModelState.IsValid)
            {
                var attendanceType = new AttendanceType
                {
                    Name = model.Name,
                    ShortName = model.ShortName,
                    ColorCode = model.ColorCode,
                    IsUserSelectable = model.IsUserSelectable,
                    RepresentsAttendance = model.RepresentsAttendance
                };
                Context.AttendanceTypes.Add(attendanceType);
                Context.SaveChanges();

                Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList(), true);
                return RedirectToAction("Types");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult EditType(int typeID)
        {
            var attendanceType = Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList()).Single(t => t.ID == typeID);
            var model = new AttendanceTypeCreation
            {
                ID = attendanceType.ID,
                Name = attendanceType.Name,
                ShortName = attendanceType.ShortName,
                ColorCode = attendanceType.ColorCode,
                IsUserSelectable = attendanceType.IsUserSelectable,
                RepresentsAttendance = attendanceType.RepresentsAttendance
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult EditType(AttendanceTypeCreation model)
        {
            if (ModelState.IsValid)
            {
                var attendanceType = Context.AttendanceTypes.Single(t => t.ID == model.ID);
                attendanceType.Name = model.Name;
                attendanceType.ShortName = model.ShortName;
                attendanceType.ColorCode = model.ColorCode;
                attendanceType.IsUserSelectable = model.IsUserSelectable;
                attendanceType.RepresentsAttendance = model.RepresentsAttendance;
                Context.SaveChanges();

                Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList(), true);
                return RedirectToAction("Types");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult DeleteType(int typeID)
        {
            if (Context.AttendanceTypes.None(t => t.ID == typeID)) return HttpNotFound();

            var model = Context.AttendanceTypes.Single(t => t.ID == typeID);
            return View(model);
        }

        [HttpPost, ActionName("DeleteType")]
        [Authorize(Roles = "Team-Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTypeConfirmed(int typeID)
        {
            if (Context.AttendanceTypes.None(t => t.ID == typeID)) return HttpNotFound();

            var model = Context.AttendanceTypes.Single(t => t.ID == typeID);
            Context.AttendanceTypes.Remove(model);
            Context.SaveChanges();
            Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList(), true);
            TempData["SuccessMessage"] = "Der Anwesenheits-Typ wurde gelöscht.";
            return RedirectToAction("Types");
        }
    }
}