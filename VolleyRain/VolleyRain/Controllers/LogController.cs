using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class LogController : BaseController
    {
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Index(int? page, int? fixedID)
        {
            var pagination = new Pagination(50, Context.Log.Count(l => !fixedID.HasValue || l.ID <= fixedID.Value), page);
            ViewBag.Pagination = pagination;

            var model = Context.Log
                .Where(l => !fixedID.HasValue || l.ID <= fixedID.Value)
                .OrderByDescending(a => a.ID)
                .Skip(pagination.ItemsToSkip)
                .Take(pagination.PageSize)
                .Select(l => new LogSummary
                {
                    ID = l.ID,
                    TimeStamp = l.TimeStamp,
                    Level = l.Level,
                    Logger = l.Logger,
                    Message = l.Message,
                    HasException = !string.IsNullOrEmpty(l.Exception)
                })
                .ToList();

            return View(model);
        }
    }
}