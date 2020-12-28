using System.Linq;
using System.Web.Mvc;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class SeasonController : BaseController
    {
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Index()
        {
            return View(Context.Seasons.OrderByDescending(s => s.Start).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Delete(int seasonID)
        {
            if (Context.Seasons.None(s => s.ID == seasonID)) return HttpNotFound();

            var model = Context.Seasons.Single(s => s.ID == seasonID);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Team-Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTypeConfirmed(int seasonID)
        {
            if (Context.Seasons.None(s => s.ID == seasonID))
            {
                return HttpNotFound();
            }

            var actualSeason = this.Cache.GetSeason(() => this.Context.Seasons.GetActualSeason());
            if (actualSeason.ID == seasonID)
            {
                TempData["ErrorMessage"] = "Die aktuelle Saison kann nicht gelöscht werden!";
                return RedirectToAction("Index");
            }

            // delete events
            var events = Context.Events.Where(e => e.Team != null && e.Team.Season.ID == seasonID).ToList();
            Context.Events.RemoveRange(events);
            Context.SaveChanges();

            // delete teams -> cascade delete

            // delete seasons
            var model = Context.Seasons.Single(s => s.ID == seasonID);
            Context.Seasons.Remove(model);
            Context.SaveChanges();

            TempData["SuccessMessage"] = "Die Saison wurde gelöscht.";
            return RedirectToAction("Index");
        }
    }
}