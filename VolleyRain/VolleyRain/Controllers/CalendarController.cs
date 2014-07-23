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
            var date = new DateTime(year, month, day);
            if (db.Events.None(e => e.Date == date)) return RedirectToAction("Create", "Event");

            return View();
        }

        private Month GetMonth(int year, int month)
        {
            var model = new Month { Date = new DateTime(year, month, 1) };
            var startOfInterval = model.Date.AddDays(-6);
            var endOfInterval = model.Date.AddMonths(1).AddDays(6);

            model.Days = db.Events
                .Where(e => e.Date >= startOfInterval && e.Date <= endOfInterval)
                .GroupBy(e => e.Date)
                .Select(g => new Day { Date = g.Key, NumberOfEvents = g.Count() })
                .ToList();
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