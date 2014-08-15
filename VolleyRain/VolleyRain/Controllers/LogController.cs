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
        public ActionResult Index(int? page, int? fixedID, string filterSession)
        {
            Func<Log, bool> predicate = null;
            if (string.IsNullOrWhiteSpace(filterSession))
            {
                predicate = l => !fixedID.HasValue || l.ID <= fixedID.Value;
            }
            else
            {
                predicate = l => l.SessionID == filterSession && (!fixedID.HasValue || l.ID <= fixedID.Value);
            }

            var pagination = new Pagination(50, Context.Log.Count(predicate), page);
            ViewBag.Pagination = pagination;

            var model = Context.Log
                .Where(predicate)
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
                    HasException = !string.IsNullOrEmpty(l.Exception),
                    SessionID = l.SessionID
                })
                .ToList();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int logID)
        {
            if (Context.Log.None(l => l.ID == logID)) return HttpNotFound();

            var model = Context.Log.Single(l => l.ID == logID);
            return View(model);
        }
    }
}