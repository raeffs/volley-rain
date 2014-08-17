using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VolleyRain.Models;

namespace VolleyRain.Helper
{
    public static class PaginationHelper
    {
        public static MvcHtmlString PreviousPageButton(this HtmlHelper htmlHelper, Pagination model, string buttonText)
        {
            return htmlHelper.PreviousPageButton(model, buttonText, string.Empty, new { });
        }

        public static MvcHtmlString PreviousPageButton(this HtmlHelper htmlHelper, Pagination model, string buttonText, string cssClass)
        {
            return htmlHelper.PreviousPageButton(model, buttonText, cssClass, new { });
        }

        public static MvcHtmlString PreviousPageButton(this HtmlHelper htmlHelper, Pagination model, string buttonText, string cssClass, object routeValues)
        {
            var aBuilder = new TagBuilder("a");
            aBuilder.AddCssClass(cssClass);
            if (model.HasPreviousPage)
            {
                var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
                var values = htmlHelper.ViewContext.RouteData.Values.Merge(new RouteValueDictionary(routeValues));
                values["page"] = model.PreviousPage;
                aBuilder.MergeAttribute("href", urlHelper.RouteUrl(values));
                aBuilder.AddCssClass("btn btn-primary btn-responsive pull-left");
            }
            else
            {
                aBuilder.MergeAttribute("href", "#");
                aBuilder.AddCssClass("btn btn-primary btn-responsive pull-left disabled");
            }
            var spanBuilder = new TagBuilder("span");
            spanBuilder.MergeAttribute("class", "glyphicon glyphicon-chevron-left");
            aBuilder.InnerHtml = spanBuilder.ToString() + " " + buttonText;
            return MvcHtmlString.Create(aBuilder.ToString());
        }

        public static MvcHtmlString NextPageButton(this HtmlHelper htmlHelper, Pagination model, string buttonText)
        {
            return htmlHelper.NextPageButton(model, buttonText, string.Empty, new { });
        }

        public static MvcHtmlString NextPageButton(this HtmlHelper htmlHelper, Pagination model, string buttonText, string cssClass)
        {
            return htmlHelper.NextPageButton(model, buttonText, cssClass, new { });
        }

        public static MvcHtmlString NextPageButton(this HtmlHelper htmlHelper, Pagination model, string buttonText, string cssClass, object routeValues)
        {
            var aBuilder = new TagBuilder("a");
            aBuilder.AddCssClass(cssClass);
            if (model.HasNextPage)
            {
                var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
                var values = htmlHelper.ViewContext.RouteData.Values.Merge(new RouteValueDictionary(routeValues));
                values["page"] = model.NextPage;
                aBuilder.MergeAttribute("href", urlHelper.RouteUrl(values));
                aBuilder.AddCssClass("btn btn-primary btn-responsive pull-right");
            }
            else
            {
                aBuilder.MergeAttribute("href", "#");
                aBuilder.AddCssClass("btn btn-primary btn-responsive pull-right disabled");
            }
            var spanBuilder = new TagBuilder("span");
            spanBuilder.MergeAttribute("class", "glyphicon glyphicon-chevron-right");
            aBuilder.InnerHtml = buttonText + " " + spanBuilder.ToString();
            return MvcHtmlString.Create(aBuilder.ToString());
        }
    }
}
