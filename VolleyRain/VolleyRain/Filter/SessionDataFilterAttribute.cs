using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.DataAccess;
using VolleyRain.Models;

namespace VolleyRain.Filter
{
    public class SessionDataFilterAttribute : ActionFilterAttribute
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Logger.Trace("Going to check whether session data is initialized...");

            var session = new SessionDecorator(filterContext.HttpContext.Session);

            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                session.Clear();
                return;
            }

            if (session.IsInitialized) return;

            Logger.Info("Session data is not yet initialized, going to load it...");

            using (var context = new DatabaseContext())
            {
                var username = filterContext.HttpContext.User.Identity.Name;
                var user = context.Users.Single(u => u.Email == username);
                session.UserID = user.ID;
                session.UserName = user.Name;
                session.Teams = user.Teams.Select(t => t.Team.ID).ToArray();
                session.IsInitialized = true;
            }
        }
    }
}