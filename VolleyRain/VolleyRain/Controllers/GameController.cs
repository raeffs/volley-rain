using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                .Select(e => new Game
                {
                    Start = e.Start,
                    HomeTeam = e.Details.HomeTeam,
                    GuestTeam = e.Details.GuestTeam,
                    Hall = e.Details.Hall,
                    IsCommited = e.Details.IsCommited,
                    SetsWonHome = e.Details.SetsWonHome,
                    SetsWonGuest = e.Details.SetsWonGuest
                })
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
                Team = team,
                Type = Context.EventTypes.Single(t => t.ID == typeID),
            };
            MapGame(game, entity);
            Context.Events.Add(entity);
        }

        private void UpdateGame(Team team, SwissVolley.GameEntry game)
        {
            var entity = Context.Events.Include(e => e.Type).Include(e => e.Details).Single(e => e.Team.ID == team.ID && e.Details.ExternalID == game.ID_game);
            MapGame(game, entity);
        }

        private void MapGame(SwissVolley.GameEntry source, Event destination)
        {
            destination.Name = string.Format("Runde {0}: {1} - {2}", source.RoundIndex, source.TeamHomeCaption.Trim(), source.TeamAwayCaption.Trim());
            destination.Start = DateTime.Parse(source.PlayDate);
            destination.End = DateTime.Parse(source.PlayDate).AddHours(3);
            if (destination.Details == null) destination.Details = new GameDetails();
            destination.Details.ExternalID = source.ID_game;
            destination.Details.HomeTeam = source.TeamHomeCaption.Trim();
            destination.Details.GuestTeam = source.TeamAwayCaption.Trim();
            destination.Details.Hall = source.HallCaption.Trim();
            destination.Details.IsCommited = source.IsResultCommited > 0;
            destination.Details.SetsWonHome = source.NumberOfWinsHome;
            destination.Details.SetsWonGuest = source.NumberOfWinsAway;
            destination.Details.Set1PointsHome = source.Set1PointsHome;
            destination.Details.Set1PointsGuest = source.Set1PointsAway;
            destination.Details.Set2PointsHome = source.Set2PointsHome;
            destination.Details.Set2PointsGuest = source.Set2PointsAway;
            destination.Details.Set3PointsHome = source.Set3PointsHome;
            destination.Details.Set3PointsGuest = source.Set3PointsAway;
            destination.Details.Set4PointsHome = source.Set4PointsHome;
            destination.Details.Set4PointsGuest = source.Set4PointsAway;
            destination.Details.Set5PointsHome = source.Set5PointsHome;
            destination.Details.Set5PointsGuest = source.Set5PointsAway;
        }
    }
}