using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace VolleyRain.Helper
{
    public static class CustomHtmlHelper
    {
        public static MvcHtmlString ColorInputFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            MvcHtmlString baseHtml = html.TextBoxFor(expression);
            return new MvcHtmlString(baseHtml.ToHtmlString().Replace("type=\"text\"", "type=\"color\""));
        }

        public static MvcHtmlString ColorInputFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            MvcHtmlString baseHtml = html.TextBoxFor(expression, htmlAttributes);
            return new MvcHtmlString(baseHtml.ToHtmlString().Replace("type=\"text\"", "type=\"color\""));
        }
    }
}