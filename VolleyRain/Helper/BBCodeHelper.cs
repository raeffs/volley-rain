using CodeKicker.BBCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VolleyRain.Helper
{
    public static class BBCodeHelper
    {
        private static readonly Lazy<BBCodeParser> Parser;

        static BBCodeHelper()
        {
            Parser = new Lazy<BBCodeParser>(() => new BBCodeParser(new[] 
            {
                new BBTag("b", "<b>", "</b>"),
                new BBTag("i", "<i>", "</i>"),
                new BBTag("url", "<a href=\"${href}\">", "</a>", new BBAttribute("href", ""), new BBAttribute("href", "href")),
            }));
        }

        public static MvcHtmlString ParseBBCode(this HtmlHelper helper, string value)
        {
            return new MvcHtmlString(Parser.Value.ToHtml(value));
        }
    }
}