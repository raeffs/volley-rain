using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.Models;

namespace VolleyRain.Helper
{
    public static class PaginationHelper
    {
        public static MvcHtmlString PreviousPageButton(this HtmlHelper htmlHelper, Pagination model, string buttonText)
        {
            var aBuilder = new TagBuilder("a");
            if (model.HasPreviousPage)
            {
                var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
                var values = htmlHelper.ViewContext.RouteData.Values;
                values["page"] = model.PreviousPage;
                aBuilder.MergeAttribute("href", urlHelper.RouteUrl(values));
                aBuilder.MergeAttribute("class", "btn btn-primary pull-left");
            }
            else
            {
                aBuilder.MergeAttribute("href", "#");
                aBuilder.MergeAttribute("class", "btn btn-primary pull-left disabled");
            }
            var spanBuilder = new TagBuilder("span");
            spanBuilder.MergeAttribute("class", "glyphicon glyphicon-chevron-left");
            aBuilder.InnerHtml = spanBuilder.ToString() + " " + buttonText;
            return MvcHtmlString.Create(aBuilder.ToString());
        }

        public static MvcHtmlString NextPageButton(this HtmlHelper htmlHelper, Pagination model, string buttonText)
        {
            var aBuilder = new TagBuilder("a");
            if (model.HasNextPage)
            {
                var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
                var values = htmlHelper.ViewContext.RouteData.Values;
                values["page"] = model.NextPage;
                aBuilder.MergeAttribute("href", urlHelper.RouteUrl(values));
                aBuilder.MergeAttribute("class", "btn btn-primary pull-right");
            }
            else
            {
                aBuilder.MergeAttribute("href", "#");
                aBuilder.MergeAttribute("class", "btn btn-primary pull-right disabled");
            }
            var spanBuilder = new TagBuilder("span");
            spanBuilder.MergeAttribute("class", "glyphicon glyphicon-chevron-right");
            aBuilder.InnerHtml = buttonText + " " + spanBuilder.ToString();
            return MvcHtmlString.Create(aBuilder.ToString());
        }
    }
}
