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
        public ActionResult Index()
        {
            return View(Context.Teams.Include(t => t.Season).OrderByDescending(t => t.Season.Start).ToList());
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.Seasons = GetSeasonSelectListItems();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(TeamCreation model)
        {
            if (ModelState.IsValid)
            {
                var team = new Team
                {
                    Name = model.Name,
                    Season = Context.Seasons.Single(s => s.ID == model.SeasonID),
                    ExternalID = model.ExternalID,
                    ExternalGroupID = model.ExternalGroupID,
                };
                Context.Teams.Add(team);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Seasons = GetSeasonSelectListItems();
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int teamID)
        {
            var model = Context.Teams.Include(t => t.Season).Select(t => new TeamEditing
                {
                    ID = t.ID,
                    Name = t.Name,
                    Season = t.Season.Name,
                    ExternalID = t.ExternalID,
                    ExternalGroupID = t.ExternalGroupID,
                })
                .Single(t => t.ID == teamID);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(TeamEditing model)
        {
            if (ModelState.IsValid)
            {
                var team = Context.Teams.Include(t => t.Season).Single(t => t.ID == model.ID);
                team.ExternalID = model.ExternalID;
                team.ExternalGroupID = model.ExternalGroupID;
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Members([TeamIdentifier] int? teamID)
        {
            var team = Context.Teams
                .Include(t => t.Members)
                .Include(t => t.Season)
                .Single(t => t.ID == teamID);
            ViewBag.Team = team;
            var members = team.Members.Select(m => m.UserID).ToList();
            var coaches = team.Members.Where(m => m.IsCoach).Select(m => m.UserID).ToList();
            var model = Context.Users
                .Include(u => u.Roles)
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .Select(u => new TeamMembershipSelection
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
                user.IsCoachOfTeam = coaches.Contains(user.UserID);
                user.IsSelf = user.UserID == Session.UserID;
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Members(int teamID, IList<TeamMembershipSelection> model)
        {
            var team = Context.Teams
                .Include(t => t.Members)
                .Include(t => t.Season)
                .Single(t => t.ID == teamID);
            ViewBag.Team = team;
            var futureMembers = model.Where(u => u.IsMemberOfTeam).Select(u => u.UserID).ToList();
            var membersToRemove = team.Members.Where(m => !futureMembers.Contains(m.UserID) && m.UserID != Session.UserID).ToList();
            membersToRemove.ForEach(m => team.Members.Remove(m));
            var currentMembers = team.Members.Select(m => m.UserID).ToList();
            var membersToAdd = Context.Users.Where(u => !currentMembers.Contains(u.ID) && futureMembers.Contains(u.ID)).ToList();
            membersToAdd.ForEach(u =>
            {
                team.Members.Add(new TeamMembership
                {
                    Team = team,
                    User = u
                });
            });
            Context.SaveChanges();

            foreach (var member in team.Members)
            {
                var item = model.SingleOrDefault(i => i.UserID == member.UserID);
                if (item == null) continue;

                member.IsCoach = item.IsCoachOfTeam;
            }
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

        public ActionResult NoTeam()
        {
            return View();
        }

        public ActionResult Select(string returnUrl)
        {
            ViewBag.ReturnUrl = new Uri(returnUrl).GetLeftPart(UriPartial.Path);
            var model = Context.Seasons.Include(s => s.Teams).ToList();
            return View(model);
        }

        private IEnumerable<SelectListItem> GetSeasonSelectListItems()
        {
            return Context.Seasons.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.ID.ToString(),
            });
        }
    }
}