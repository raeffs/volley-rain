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
    public class EventController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] EventCreation eventCreation)
        {
            if (ModelState.IsValid)
            {
                var entity = new Event
                {
                    Name = eventCreation.Name,
                    Description = eventCreation.Description,
                    Location = eventCreation.Location,
                    Type = eventCreation.Type,
                    Start = eventCreation.StartDate,
                    End = eventCreation.EndDate
                };

                db.Events.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index", "Calendar");
            }

            return View(eventCreation);
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
