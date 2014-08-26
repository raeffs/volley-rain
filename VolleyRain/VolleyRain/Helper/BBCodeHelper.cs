using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VolleyRain.Helper
{
    public static class BBCodeHelper
    {
        public static MvcHtmlString ParseBBCode(this HtmlHelper helper, string value)
        {
            var parser = new CodeKicker.BBCode.BBCodeParser(new[] 
            {
                new CodeKicker.BBCode.BBTag("b", "<b>", "</b>")
            });
            return new MvcHtmlString(parser.ToHtml(value));
        }
    }
}