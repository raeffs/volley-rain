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

            //routes.MapRoute(
            //    name: "AttendanceIndex",
            //    url: "Attendance/{team}",
            //    defaults: new { controller = "Attendance", action = "Index", team = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "LogToggleLevel",
                url: "Log/ToggleLevel/{level}",
                defaults: new { controller = "Log", action = "ToggleLevel" }
            );

            routes.MapRoute(
                name: "LogDetails",
                url: "Log/Details/{logID}",
                defaults: new { controller = "Log", action = "Details" }
            );

            routes.MapRoute(
                name: "Log",
                url: "Log/{page}/{fixedID}/{filterSession}",
                defaults: new { controller = "Log", action = "Index", page = UrlParameter.Optional, fixedID = UrlParameter.Optional, filterSession = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CalendarExport",
                url: "Calendar/Export",
                defaults: new { controller = "Calendar", action = "Export" }
            );

            routes.MapRoute(
                name: "EventDetails",
                url: "Event/Details/{eventID}",
                defaults: new { controller = "Event", action = "Details" }
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
