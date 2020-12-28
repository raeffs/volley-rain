using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VolleyRain.Filter
{
    public class ConditionalFilterProvider : IFilterProvider
    {
        private readonly IEnumerable<Func<ControllerContext, ActionDescriptor, object>> _conditions;

        public ConditionalFilterProvider(IEnumerable<Func<ControllerContext, ActionDescriptor, object>> conditions)
        {
            _conditions = conditions;
        }

        IEnumerable<System.Web.Mvc.Filter> IFilterProvider.GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return _conditions
                .Select(condition => condition(controllerContext, actionDescriptor))
                .Where(filter => filter != null)
                .Select(filter => new System.Web.Mvc.Filter(filter, FilterScope.Global, null));
        }
    }
}