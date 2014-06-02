using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.DataAccess;

namespace VolleyRain.Controllers
{
    public class RankingController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Rankings.ToList());
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