using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VolleyRain.Models;
using VolleyRain.DataAccess;

namespace VolleyRain.Controllers
{
    public class EventController : BaseController
    {
        public EventController()
            : base()
        {
            ViewBag.EventTypes = new SelectList(Context.EventTypes, "ID", "Name");
        }

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Create(string dateHint)
        {
            var temp = default(DateTime);
            var date = DateTime.TryParse(dateHint, out temp) ? temp.Date : DateTime.Now.Date;

            var model = new EventCreation
            {
                FullTime = true,
                StartDate = date,
                EndDate = date,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Team-Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventCreation model)
        {
            // TODO: geht bestimmt eleganter
            var selectedType = Context.EventTypes.SingleOrDefault(t => t.ID == model.Type.ID);
            if (selectedType == null)
            {
                ModelState.AddModelError("Type", "Der gewählte Typ existiert nicht.");
            }
            if (!model.FullTime && !model.StartTime.HasValue)
            {
                ModelState.AddModelError("StartTime", "Das Feld \"Zeit (von)\" wird benötigt.");
            }
            if (!model.FullTime && !model.EndTime.HasValue)
            {
                ModelState.AddModelError("StartTime", "Das Feld \"Zeit (bis)\" wird benötigt.");
            }
            if (model.RecurrsWeekly && !model.NumberOfRecurrences.HasValue)
            {
                ModelState.AddModelError("NumberOfRecurrences", "Das Feld \"Anzahl Wiederholungen\" wird benötigt.");
            }

            if (ModelState.IsValid)
            {
                var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
                var entity = new Event
                {
                    Name = model.Name,
                    Description = model.Description,
                    Location = model.Location,
                    Type = selectedType,
                    Team = Context.Teams.Include(t => t.Members).Single(t => t.Season.ID == season.ID)
                };

                if (model.FullTime)
                {
                    entity.Start = model.StartDate.Date;
                    entity.End = model.EndDate.Date.AddDays(1);
                }
                else
                {
                    entity.Start = model.StartDate.Date.Add(model.StartTime.Value.TimeOfDay);
                    entity.End = model.EndDate.Date.Add(model.EndTime.Value.TimeOfDay);
                }

                Context.Events.Add(entity);

                if (model.RecurrsWeekly)
                {
                    for (int i = 1; i <= model.NumberOfRecurrences; i++)
                    {
                        var recurrence = new Event
                        {
                            Name = entity.Name,
                            Description = entity.Description,
                            Location = entity.Location,
                            Type = entity.Type,
                            Team = entity.Team,
                            Start = entity.Start.AddDays(7 * i),
                            End = entity.End.AddDays(7 * i),
                        };
                        Context.Events.Add(recurrence);
                    }
                }

                Context.SaveChanges();
                return RedirectToAction("Index", "Calendar");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Delete(int eventID)
        {
            if (Context.Events.None(e => e.ID == eventID)) return HttpNotFound();

            var model = Context.Events.Single(e => e.ID == eventID);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Team-Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int eventID)
        {
            if (Context.Events.None(e => e.ID == eventID)) return HttpNotFound();

            var model = Context.Events.Single(e => e.ID == eventID);
            Context.Events.Remove(model);
            Context.SaveChanges();
            TempData["SuccessMessage"] = "Der Termin wurde gelöscht.";
            return RedirectToAction("Index", "Calendar");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Details(int eventID)
        {
            if (Context.Events.None(e => e.ID == eventID)) return HttpNotFound();

            var model = Context.Events
                .Select(e => new EventDetails
                {
                    ID = e.ID,
                    Name = e.Name,
                    Description = e.Description,
                    Location = e.Location,
                    TypeName = e.Type.Name,
                    Start = e.Start,
                    End = e.End
                })
                .Single(e => e.ID == eventID);
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Attendance(int eventID)
        {
            if (Context.Events.None(e => e.ID == eventID)) return HttpNotFound();

            var noSelection = new AttendanceType { ID = 0, Name = string.Empty };
            var attendanceTypes = Cache.GetAttendanceTypes(() => Context.AttendanceTypes.ToList()).ToList();
            attendanceTypes.Add(noSelection);
            ViewBag.AttendanceTypes = attendanceTypes.OrderBy(t => t.ID).ToList();

            var teamID = Context.Events.Single(e => e.ID == eventID).Team.ID;

            var model = Context.Users
                .OrderByDescending(u => u.Teams.Any(t => t.IsCoach))
                .ThenBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .Where(u => u.Teams.Any(t => t.TeamID == teamID))
                .Select(u => new EffectiveAttendanceSelection
                {
                    UserID = u.ID,
                    UserName = u.Name,
                    UserSurname = u.Surname,
                })
                .ToList();

            foreach (var attendance in Context.Attendances.Include(a => a.User).Include(a => a.Type).Where(a => a.Event.ID == eventID))
            {
                var m = model.Single(u => u.UserID == attendance.User.ID);
                m.AttendanceType = attendance.Type;
                m.Comment = attendance.Comment;
            }
            foreach (var m in model)
            {
                if (m.AttendanceType == null) m.AttendanceType = noSelection;
            }

            return PartialView(model);
        }

        [HttpPost]
        [Authorize(Roles = "Team-Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Attendance(int eventID, IList<EffectiveAttendanceSelection> model)
        {
            if (Context.Events.None(e => e.ID == eventID)) return HttpNotFound();

            var attendanceTypes = Context.AttendanceTypes.ToList();
            var teamID = Context.Events.Single(e => e.ID == eventID).Team.ID;
            var users = Context.Users.Where(u => u.Teams.Any(t => t.TeamID == teamID)).ToList();
            var attendances = Context.Attendances.Where(a => a.Event.ID == eventID).ToList();
            var toRemove = new List<Attendance>();

            foreach (var item in model)
            {
                var type = attendanceTypes.SingleOrDefault(t => t.ID == item.AttendanceType.ID);
                if (attendances.Any(a => a.User.ID == item.UserID))
                {
                    if (type == null)
                    {
                        toRemove.Add(attendances.Single(a => a.User.ID == item.UserID));
                    }
                    else
                    {
                        var attendance = attendances.Single(a => a.User.ID == item.UserID);
                        attendance.Type = type;
                        attendance.Comment = item.Comment;
                    }
                }
                else
                {
                    if (type == null) continue;

                    Context.Attendances.Add(new Attendance
                    {
                        User = Context.Users.Single(u => u.ID == item.UserID),
                        Event = Context.Events.Single(e => e.ID == eventID),
                        Type = type,
                        Comment = item.Comment,
                    });
                }
            }
            Context.Attendances.RemoveRange(toRemove);
            Context.SaveChanges();

            TempData["SuccessMessage"] = "Daten wurden gespeichert.";
            return RedirectToAction("Details", new { eventID = eventID });
        }
    }
}
