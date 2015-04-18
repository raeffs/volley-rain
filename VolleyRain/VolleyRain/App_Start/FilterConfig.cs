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
            filters.Add(new TeamSelectionFilter()); // TODO explizit bzw. conditional
            filters.Add(new ExceptionLoggingAttribute());
        }

        public static void RegisterFilterProviders(FilterProviderCollection providers)
        {
            var conditions = new Func<ControllerContext, ActionDescriptor, object>[] 
            {
                (c, a) => c.Controller.GetType() != typeof(NavigationController) && c.Controller.GetType() != typeof(StyleController) ? new ActionLoggingAttribute() : null,
            };
            providers.Add(new ConditionalFilterProvider(conditions));
        }
    }
}