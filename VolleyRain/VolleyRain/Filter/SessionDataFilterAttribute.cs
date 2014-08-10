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
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = new SessionDecorator(filterContext.HttpContext.Session);

            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                session.Clear();
                return;
            }

            if (session.IsInitialized) return;

            using (var context = new DatabaseContext())
            {
                var username = filterContext.HttpContext.User.Identity.Name;
                var user = context.Users.Single(u => u.Email == username);
                session.UserID = user.ID;
                session.UserName = user.Name;
                session.Teams = user.Teams.Select(t => t.ID).ToArray();
                session.IsInitialized = true;
            }
        }
    }
}