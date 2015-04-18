using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VolleyRain.Models;
using VolleyRain.DataAccess;

namespace VolleyRain.Controllers
{
    public class NewsController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? page)
        {
            var pagination = new Pagination(5, Context.NewsArticles.Count(a => a.IsPublic), page);
            ViewBag.Pagination = pagination;

            var model = Context.NewsArticles
                .Where(a => a.IsPublic)
                .OrderByDescending(a => a.PublishDate)
                .Skip(pagination.ItemsToSkip)
                .Take(pagination.PageSize)
                .ToList();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Team-Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsCreation model)
        {
            if (ModelState.IsValid)
            {
                var entity = new NewsArticle
                {
                    Title = model.Title,
                    Content = model.Content,
                    PublishDate = DateTime.Now,
                };
                Context.NewsArticles.Add(entity);
                Context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Edit(int newsID)
        {
            if (Context.NewsArticles.None(n => n.ID == newsID)) return HttpNotFound();

            var model = Context.NewsArticles.Single(n => n.ID == newsID);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Team-Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsArticle model)
        {
            if (ModelState.IsValid)
            {
                var entity = Context.NewsArticles.Single(n => n.ID == model.ID);
                entity.Title = model.Title;
                entity.Content = model.Content;
                Context.SaveChanges();

                TempData["successMessage"] = "Änderungen wurden gespeichert.";
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Delete(int newsID)
        {
            if (Context.NewsArticles.None(n => n.ID == newsID)) return HttpNotFound();

            var model = Context.NewsArticles.Single(n => n.ID == newsID);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Team-Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int newsID)
        {
            if (Context.NewsArticles.None(n => n.ID == newsID)) return HttpNotFound();

            var model = Context.NewsArticles.Single(n => n.ID == newsID);
            Context.NewsArticles.Remove(model);
            Context.SaveChanges();
            TempData["SuccessMessage"] = "Die News wurden gelöscht.";
            return RedirectToAction("Index");
        }
    }
}
