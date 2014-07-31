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
            var games = Context.Events
                .Where(e => e.Team.Season.ID == season.ID && e.Details != null)
                .Select(e => new Game { Start = e.Start, HomeTeam = e.Details.HomeTeam, GuestTeam = e.Details.GuestTeam, Hall = e.Details.Hall })
                .ToList();

            return View(games);
        }

        public void Update(int? teamID)
        {
            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var teams = Context.Teams.Where(t => t.Season.ID == season.ID).ToList();
            if (teamID.HasValue) teams = teams.Where(t => t.ID == teamID).ToList();

            foreach (var team in teams)
            {
                UpdateGames(team);
            }
        }

        private void UpdateGames(Team team)
        {
            var games = new SwissVolley.SwissVolleyPortTypeClient().getGamesTeam(team.ExternalID);
            foreach (var game in games)
            {
                if (Context.GameDetails.Any(g => g.ExternalID == game.ID_game && g.Event.Team.ID == team.ID))
                {
                    UpdateGame(team, game);
                }
                else
                {
                    CreateGame(team, game);
                }
            }
            Context.SaveChanges();
        }

        private void CreateGame(Team team, SwissVolley.GameEntry game)
        {
            // TODO: do it the right way
            var typeID = game.TeamHomeID == team.ExternalID ? 2 : 3;
            var entity = new Event
            {
                Name = string.Format("Runde {0}: {1} - {2}", game.RoundIndex, game.TeamHomeCaption, game.TeamAwayCaption),
                Start = DateTime.Parse(game.PlayDate),
                End = DateTime.Parse(game.PlayDate).AddHours(3),
                Team = team,
                Type = Context.EventTypes.Single(t => t.ID == typeID),
                Details = new GameDetails
                {
                    ExternalID = game.ID_game,
                    HomeTeam = game.TeamHomeCaption,
                    GuestTeam = game.TeamAwayCaption,
                    Hall = game.HallCaption,
                }
            };
            Context.Events.Add(entity);
        }

        private void UpdateGame(Team team, SwissVolley.GameEntry game)
        {
            // TODO: do it
        }
    }
}