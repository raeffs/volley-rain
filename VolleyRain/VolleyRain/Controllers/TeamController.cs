using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.Models;
using System.Data.Entity;

namespace VolleyRain.Controllers
{
    public class TeamController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Members(int? teamID)
        {
            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var team = Context.Teams.Include(t => t.Members).Single(t => t.Season.ID == season.ID);
            var members = team.Members.Select(t => t.ID).ToList();
            var model = Context.Users
                .Select(u => new TeamMembership { UserID = u.ID, Name = u.Name, Surname = u.Surname })
                .ToList();

            foreach (var user in model)
            {
                user.IsMemberOfTeam = members.Contains(user.UserID);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Members(int? teamID, IList<TeamMembership> model)
        {
            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var team = Context.Teams.Include(t => t.Members).Single(t => t.Season.ID == season.ID);
            var futureMembers = model.Where(u => u.IsMemberOfTeam).Select(u => u.UserID).ToList();
            var toRemove = team.Members.Where(u => !futureMembers.Contains(u.ID)).ToList();
            toRemove.ForEach(u => team.Members.Remove(u));
            var currentMembers = team.Members.Select(u => u.ID).ToList();
            var toAdd = Context.Users.Where(u => !currentMembers.Contains(u.ID) && futureMembers.Contains(u.ID)).ToList();
            toAdd.ForEach(u => team.Members.Add(u));
            Context.SaveChanges();

            return Members(teamID);
        }
    }
}