using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.DataAccess;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class CalendarController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

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

            return View(GetMonth(year.Value, month.Value));
        }

        public ActionResult Details(int year, int month, int day)
        {
            var model = new Day(year, month, day);
            if (db.Events.None(e => e.End >= model.CalendarViewPeriod.Start && e.Start <= model.CalendarViewPeriod.End)) return RedirectToAction("Create", "Event");

            return View();
        }

        private Month GetMonth(int year, int month)
        {
            var model = new Month(year, month);

            var events = db.Events
                .Where(e => e.End >= model.CalendarViewPeriod.Start && e.Start <= model.CalendarViewPeriod.End)
                .ToList()
                .Select(e => new Itenso.TimePeriod.TimeRange(e.Start, e.End, true));

            foreach (var day in model.Days)
            {
                day.NumberOfEvents = events.Count(e => day.CalendarViewPeriod.OverlapsWith(e));
            }

            return model;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}