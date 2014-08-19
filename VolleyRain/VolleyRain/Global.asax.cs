using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using VolleyRain.DataAccess;
using VolleyRain.Models;
using VolleyRain.Security;

namespace VolleyRain
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            Logger.Info("Application is starting...");
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected void Application_End()
        {
            try
            {
                var runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember(
                    "_theRuntime",
                    BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField,
                    null,
                    null,
                    null);
                var shutdownReason = (ApplicationShutdownReason)runtime.GetType().InvokeMember(
                    "_shutdownReason",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                    null,
                    runtime,
                    null);
                Logger.Warn("Application is shutting down! Shutdown reason: [{0}]", shutdownReason);
            }
            catch (Exception exception)
            {
                Logger.Error("Could not determine shutdown reason!", exception);
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs args)
        {
            if (Request.IsAuthenticated)
            {
                var identity = new CustomIdentity(HttpContext.Current.User.Identity);
                var principal = new CustomPrincipal(identity);
                HttpContext.Current.User = principal;
            }
        }
    }
}
