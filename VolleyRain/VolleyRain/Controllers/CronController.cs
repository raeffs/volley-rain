using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class CronController : BaseController
    {
        public void Index()
        {
        }

        public void UpdateMatches(int? teamID)
        {
            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var teams = season.Teams;
            if (teamID.HasValue) teams = teams.Where(t => t.ID == teamID).ToList();

            foreach (var team in teams)
            {
                var client = new SwissVolley.SwissVolleyPortTypeClient();
                var matches = client.getGamesTeam(team.ExternalID);
            }
        }
    }
}