using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.DataAccess;
using VolleyRain.Filter;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class CalendarController : BaseController
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index(int? year, int? month)
        {
            if (!year.HasValue || year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
            {
                year = DateTime.Today.Year;
            }
            if (!month.HasValue || month < DateTime.MinValue.Month || month > DateTime.MaxValue.Month)
            {
                month = DateTime.Today.Month;
            }

            var model = new Month(year.Value, month.Value);
            var events = Context.Events
                .Where(e => e.End >= model.CalendarViewPeriod.Start && e.Start <= model.CalendarViewPeriod.End && (e.Team == null || Session.Teams.Contains(e.Team.ID)))
                .Select(e => new EventSummary
                {
                    ID = e.ID,
                    Name = e.Name,
                    Start = e.Start,
                    End = e.End,
                    TypeID = e.Type.ID
                })
                .ToList();
            foreach (var day in model.Days)
            {
                day.Events = events.Where(e => day.CalendarViewPeriod.OverlapsWith(new Itenso.TimePeriod.TimeRange(e.Start, e.End))).ToList();
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Details(int year, int month, int day)
        {
            var model = new Day(year, month, day);
            if (Context.Events.None(e => e.End >= model.CalendarViewPeriod.Start && e.Start <= model.CalendarViewPeriod.End)) return RedirectToAction("Create", "Event");

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Export()
        {
            var model = Context.EventTypes
                .Select(t => new CalendarExportOption
                {
                    ID = t.ID,
                    EventType = t,
                    Export = true
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ConvertViewToFile("text/calendar", "Calendar.ics")]
        public ActionResult Export(IList<CalendarExportOption> model)
        {
            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var teamIDs = Context.Teams.Where(t => t.Season.ID == season.ID && Session.Teams.Contains(t.ID)).Select(t => t.ID).ToList();
            var typeIDs = model.Where(m => m.Export).Select(m => m.ID);

            var events = Context.Events
                .Where(e => teamIDs.Contains(e.Team.ID) && typeIDs.Contains(e.Type.ID))
                .OrderBy(e => e.Start)
                .ToList();

            return PartialView("Export.ics", events);
        }
    }
}