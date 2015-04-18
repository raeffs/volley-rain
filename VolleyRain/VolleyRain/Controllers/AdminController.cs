using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VolleyRain.Controllers
{
    public class AdminController : BaseController
    {
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Reset()
        {
            return View();
        }

        [HttpPost, ActionName("Reset")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmReset()
        {
            Context.Attendances.RemoveRange(Context.Attendances);
            Context.Documents.RemoveRange(Context.Documents);
            Context.Events.RemoveRange(Context.Events);
            Context.GameDetails.RemoveRange(Context.GameDetails);
            Context.NewsArticles.RemoveRange(Context.NewsArticles);
            Context.Rankings.RemoveRange(Context.Rankings);
            Context.TeamMemberships.RemoveRange(Context.TeamMemberships);
            Context.Teams.RemoveRange(Context.Teams);
            Context.SaveChanges();
            return View();
        }
    }
}