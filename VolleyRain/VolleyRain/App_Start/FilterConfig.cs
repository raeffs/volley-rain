using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.Controllers;
using VolleyRain.Filter;

namespace VolleyRain
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SessionDataFilterAttribute());
            filters.Add(new ActionLoggingAttribute());
            filters.Add(new ExceptionLoggingAttribute());
        }
    }
}