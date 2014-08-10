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

        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Create()
        {
            var model = new EventCreation
            {
                FullTime = true,
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date,
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
    }
}
