using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class GameController : BaseController
    {
        public ActionResult Index(int? teamID)
        {
            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var games = Context.Events.Where(e => e.Team.Season.ID == season.ID && e.Details != null).ToList();

            return View(games);
        }
    }
}