using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VolleyRain.Filter
{
    public class ConvertViewToFileAttribute : ActionFilterAttribute
    {
        public ConvertViewToFileAttribute(string contentType, string filename)
        {
            ContentType = contentType;
            Filename = filename;
        }

        public string ContentType { get; private set; }

        public string Filename { get; private set; }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.ContentType = ContentType;
            response.AddHeader("Content-Disposition", string.Format("filename=\"{0}\"", Filename));
        }
    }
}