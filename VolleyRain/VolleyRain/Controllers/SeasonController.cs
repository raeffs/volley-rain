using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VolleyRain.Controllers
{
    public class SeasonController : BaseController
    {
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Index()
        {
            return View(Context.Seasons.OrderByDescending(s => s.Start).ToList());
        }
    }
}