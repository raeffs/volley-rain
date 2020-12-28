using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VolleyRain.Filter
{
    public class ExceptionLoggingAttribute : HandleErrorAttribute
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void OnException(ExceptionContext filterContext)
        {
            LogException(filterContext);
            base.OnException(filterContext);
        }

        private void LogException(ExceptionContext filterContext)
        {
            Logger.Error(filterContext.Exception.Message, filterContext.Exception);
        }
    }
}