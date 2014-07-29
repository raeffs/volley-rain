using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.DataAccess;
using VolleyRain.Models;

namespace VolleyRain.Filter
{
    public class SessionDataFilter : ActionFilterAttribute
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
                session.Teams = context.Users.Single(u => u.Username == username).Teams.Select(t => t.ID).ToArray();
                session.IsInitialized = true;
            }
        }
    }
}