using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.DataAccess;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class RankingController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(Context.Rankings.OrderBy(r => r.Rank).ToList());
        }

        public void Update(int? teamID)
        {
            var season = Cache.GetSeason(() => Context.Seasons.GetActualSeason());
            var teams = Context.Teams.Where(t => t.Season.ID == season.ID).ToList();
            if (teamID.HasValue) teams = teams.Where(t => t.ID == teamID).ToList();

            foreach (var team in teams)
            {
                UpdateRankings(team);
            }
        }

        private void UpdateRankings(Team team)
        {
            var rank = 0;
            var rankings = new SwissVolley.SwissVolleyPortTypeClient().getTable(team.ExternalGroupID);
            foreach (var ranking in rankings)
            {
                if (IsRank(ranking.Rank)) rank++;
                if (Context.Rankings.Any(r => r.ExternalID == ranking.team_ID && r.AssociatedTeam.ID == team.ID))
                {
                    UpdateRanking(team, ranking, rank);
                }
                else
                {
                    CreateRanking(team, ranking, rank);
                }
            }
            Context.SaveChanges();
        }

        private bool IsRank(string value)
        {
            int temp;
            return int.TryParse(value.Trim('.'), out temp);
        }

        private void CreateRanking(Team team, SwissVolley.TableEntry ranking, int rank)
        {
            var entity = new Ranking
            {
                ExternalID = ranking.team_ID,
                AssociatedTeam = team,
                Rank = rank,
                Team = ranking.Caption,
                NumberOfGames = ranking.NumberOfGames,
                SetsWon = ranking.SetsWon,
                SetsLost = ranking.SetsLost,
                SetQuotient = (decimal)ranking.SetQuotient,
                BallsWon = ranking.BallsWon,
                BallsLost = ranking.BallsLost,
                BallQuotient = (decimal)ranking.BallsQuotient,
                Points = ranking.Points,
            };
            Context.Rankings.Add(entity);
        }

        private void UpdateRanking(Team team, SwissVolley.TableEntry ranking, int rank)
        {
            var entity = Context.Rankings.Single(r => r.ExternalID == ranking.team_ID && r.AssociatedTeam.ID == team.ID);
            entity.Rank = rank;
            entity.Team = ranking.Caption;
            entity.NumberOfGames = ranking.NumberOfGames;
            entity.SetsWon = ranking.SetsWon;
            entity.SetsLost = ranking.SetsLost;
            entity.SetQuotient = (decimal)ranking.SetQuotient;
            entity.BallsWon = ranking.BallsWon;
            entity.BallsLost = ranking.BallsLost;
            entity.BallQuotient = (decimal)ranking.BallsQuotient;
            entity.Points = ranking.Points;
        }
    }
}