using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VolleyRain
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AttendanceIndex",
                url: "Attendance/{team}",
                defaults: new { controller = "Attendance", action = "Index", team = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CalendarDetails",
                url: "Calendar/{year}/{month}/{day}",
                defaults: new { controller = "Calendar", action = "Details" }
            );

            routes.MapRoute(
                name: "CalendarIndex",
                url: "Calendar/{year}/{month}",
                defaults: new { controller = "Calendar", action = "Index", year = UrlParameter.Optional, month = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "News", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
