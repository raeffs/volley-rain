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

        public ActionResult Details(int year, int month, int day)
        {
            var model = new Day(year, month, day);
            if (Context.Events.None(e => e.End >= model.CalendarViewPeriod.Start && e.Start <= model.CalendarViewPeriod.End)) return RedirectToAction("Create", "Event");

            return View();
        }

        [TestFilter]
        public ActionResult Export()
        {
            return View();
        }
    }
}