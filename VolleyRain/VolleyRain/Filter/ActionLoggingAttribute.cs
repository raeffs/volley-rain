using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace VolleyRain.Filter
{
    public class ActionLoggingAttribute : ActionFilterAttribute
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Logger.IsDebugEnabled) return;

            var builder = new StringBuilder();
            builder.AppendFormat(
                "Action [{0}.{1}] is executed",
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName);
            if (filterContext.ActionParameters.Count > 0)
            {
                builder.Append(" with parameters [");
                foreach (var parameter in filterContext.ActionParameters)
                {
                    builder.AppendFormat("{0} = {1}, ", parameter.Key, parameter.Value);
                }
                builder.Remove(builder.Length - 2, 2);
                builder.Append("]");
            }
            Logger.Debug(builder.ToString());
        }
    }
}