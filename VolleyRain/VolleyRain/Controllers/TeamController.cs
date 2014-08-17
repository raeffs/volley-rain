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
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Members(int? teamID)
        {
            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var team = Context.Teams.Include(t => t.Members).Single(t => t.Season.ID == season.ID);
            var members = team.Members.Select(t => t.ID).ToList();
            var model = Context.Users
                .Include(u => u.Roles)
                .OrderBy(u => u.Name)
                .Select(u => new TeamMembership
                {
                    UserID = u.ID,
                    Name = u.Name,
                    Email = u.Email,
                    Surname = u.Surname,
                    IsAdminOfTeam = u.Roles.Any(r => r.IsDefaultTeamAdminRole)
                })
                .ToList();

            foreach (var user in model)
            {
                user.IsMemberOfTeam = members.Contains(user.UserID);
                user.IsSelf = user.UserID == Session.UserID;
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
            var membersToRemove = team.Members.Where(u => !futureMembers.Contains(u.ID) && u.ID != Session.UserID).ToList();
            membersToRemove.ForEach(u => team.Members.Remove(u));
            var currentMembers = team.Members.Select(u => u.ID).ToList();
            var membersToAdd = Context.Users.Where(u => !currentMembers.Contains(u.ID) && futureMembers.Contains(u.ID)).ToList();
            membersToAdd.ForEach(u => team.Members.Add(u));
            Context.SaveChanges();

            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsAdministrator())
            {
                var adminRole = Context.Roles.Single(r => r.IsDefaultTeamAdminRole);

                var futureAdmins = model.Where(u => u.IsAdminOfTeam).Select(u => u.UserID).ToList();
                var adminsToRemove = adminRole.Users.Where(u => !futureAdmins.Contains(u.ID) && u.ID != Session.UserID).ToList();
                adminsToRemove.ForEach(u => u.Roles.Remove(adminRole));
                var currentAdmins = adminRole.Users.Select(u => u.ID).ToList();
                var adminsToAdd = Context.Users.Where(u => !currentAdmins.Contains(u.ID) && futureAdmins.Contains(u.ID)).ToList();
                adminsToAdd.ForEach(u => u.Roles.Add(adminRole));
                Context.SaveChanges();
            }

            return Members(teamID);
        }
    }
}