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

        public ActionResult Index()
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var startOfInterval = startOfMonth.AddDays(-6);
            var endOfInterval = startOfMonth.AddMonths(1).AddDays(6);

            var days = db.Events
                .Where(e => e.Date >= startOfInterval && e.Date <= endOfInterval)
                .GroupBy(e => e.Date)
                .Select(g => new DaySummary { Date = g.Key, NumberOfEvents = g.Count() })
                .ToList();

            return View(days);
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