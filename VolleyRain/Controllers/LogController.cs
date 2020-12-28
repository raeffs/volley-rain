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
            var enabledLogLevels = GetEnabledLogLevels();
            Func<Log, bool> predicate = null;
            if (string.IsNullOrWhiteSpace(filterSession))
            {
                predicate = l => enabledLogLevels.Contains(l.Level) && (!fixedID.HasValue || l.ID <= fixedID.Value);
            }
            else
            {
                predicate = l => enabledLogLevels.Contains(l.Level) && l.SessionID == filterSession && (!fixedID.HasValue || l.ID <= fixedID.Value);
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

        private List<string> GetEnabledLogLevels()
        {
            var levels = new List<string>();
            if (Session.ShowLogLevelFatal) levels.Add("Fatal");
            if (Session.ShowLogLevelError) levels.Add("Error");
            if (Session.ShowLogLevelWarn) levels.Add("Warn");
            if (Session.ShowLogLevelInfo) levels.Add("Info");
            if (Session.ShowLogLevelDebug) levels.Add("Debug");
            if (Session.ShowLogLevelTrace) levels.Add("Trace");
            return levels;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int logID)
        {
            if (Context.Log.None(l => l.ID == logID)) return HttpNotFound();

            var model = Context.Log.Single(l => l.ID == logID);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult ToggleLevel(string level)
        {
            switch (level)
            {
                case "Fatal":
                    Session.ShowLogLevelFatal = !Session.ShowLogLevelFatal;
                    break;
                case "Error":
                    Session.ShowLogLevelError = !Session.ShowLogLevelError;
                    break;
                case "Warn":
                    Session.ShowLogLevelWarn = !Session.ShowLogLevelWarn;
                    break;
                case "Info":
                    Session.ShowLogLevelInfo = !Session.ShowLogLevelInfo;
                    break;
                case "Debug":
                    Session.ShowLogLevelDebug = !Session.ShowLogLevelDebug;
                    break;
                case "Trace":
                    Session.ShowLogLevelTrace = !Session.ShowLogLevelTrace;
                    break;
            }
            return RedirectToAction("Index");
        }
    }
}