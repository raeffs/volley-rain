using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using VolleyRain.DataAccess;
using VolleyRain.Models;

namespace VolleyRain.Filter
{
    public class TeamSelectionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionMethod = GetActionMethod(filterContext);
            if (actionMethod == null) return;
            var teamParameter = actionMethod.GetParameters().FirstOrDefault(p => p.HasCustomAttributes<TeamIdentifierAttribute>());
            if (teamParameter == null) return;

            var cache = new CacheDecorator(filterContext.HttpContext.Cache);
            var session = new SessionDecorator(filterContext.HttpContext.Session);

            var value = filterContext.ActionParameters[teamParameter.Name] as int?;
            if (!value.HasValue) value = session.SelectedTeamID;

            if (actionMethod.HasCustomAttributes<AllowAnonymousAttribute>() || filterContext.HttpContext.User.IsAdministrator())
            {
                if (!value.HasValue || !TeamExists(value.Value))
                {
                    value = GetTeamOfActualSeason(cache);
                }
                if (!value.HasValue)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { 
                        { "Controller", "Team" }, 
                        { "Action", "Select" } ,
                        { "ReturnUrl", filterContext.RequestContext.HttpContext.Request.Url }
                    });
                }
            }
            else
            {
                if (!value.HasValue || !IsMemberOfTeam(value.Value, session))
                {
                    value = GetTeamOfActualSeason(filterContext, cache, session);
                }
            }

            if (value.HasValue) session.SelectedTeamID = value;
            filterContext.ActionParameters[teamParameter.Name] = value;
        }

        private int? GetTeamOfActualSeason(ActionExecutingContext filterContext, CacheDecorator cache, SessionDecorator session)
        {
            using (var db = new DatabaseContext())
            {
                var season = cache.GetSeason(() => db.Seasons.GetActualSeason());
                var teams = db.Teams.Where(t => t.Season.ID == season.ID && t.Members.Any(m => m.UserID == session.UserID)).ToList();
                if (teams.Count == 1)
                {
                    return teams.Single().ID;
                }
                else if (teams.Count > 1)
                {
                    // select team
                }
                else if (teams.Count == 0)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new
                    {
                        controller = "Team",
                        action = "NoTeam"
                    }));
                }
                return null;
            }
        }

        private bool IsMemberOfTeam(int teamID, SessionDecorator session)
        {
            using (var db = new DatabaseContext())
            {
                return db.TeamMemberships.Any(m => m.TeamID == teamID && m.UserID == session.UserID);
            }
        }

        private int? GetTeamOfActualSeason(CacheDecorator cache)
        {
            using (var db = new DatabaseContext())
            {
                var season = cache.GetSeason(() => db.Seasons.GetActualSeason());
                var teams = db.Teams.Where(t => t.Season.ID == season.ID);
                return teams.Count() == 1 ? teams.Single().ID : default(int?);
            }
        }

        private bool TeamExists(int teamID)
        {
            using (var db = new DatabaseContext())
            {
                return db.Teams.Any(t => t.ID == teamID);
            }
        }

        private MethodInfo GetActionMethod(ActionExecutingContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.ActionName;
            var controllerType = filterContext.Controller.GetType();
            var actionMethod = default(MethodInfo);
            try
            {
                actionMethod = controllerType.GetMethod(actionName);
            }
            catch (AmbiguousMatchException)
            {
                var parameters = filterContext.ActionParameters.Select(p => p.GetType()).ToArray();
                actionMethod = controllerType.GetMethod(actionName, parameters);
            }
            if (actionMethod == null)
            {
                foreach (var method in controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (string.Equals(method.Name, actionName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        actionMethod = method;
                        break;
                    }
                }
            }
            return actionMethod;
        }
    }
}