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
        public ActionResult Index(int? page)
        {
            var pagination = new Pagination(5, Context.NewsArticles.Count(), page);
            ViewBag.Pagination = pagination;

            var model = Context.NewsArticles
                .OrderByDescending(a => a.PublishDate)
                .Skip(pagination.ItemsToSkip)
                .Take(pagination.PageSize)
                .ToList();

            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticle newsarticle = Context.NewsArticles.Find(id);
            if (newsarticle == null)
            {
                return HttpNotFound();
            }
            return View(newsarticle);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Content,PublishDate")] NewsArticle newsarticle)
        {
            if (ModelState.IsValid)
            {
                Context.NewsArticles.Add(newsarticle);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newsarticle);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticle newsarticle = Context.NewsArticles.Find(id);
            if (newsarticle == null)
            {
                return HttpNotFound();
            }
            return View(newsarticle);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Content,PublishDate")] NewsArticle newsarticle)
        {
            if (ModelState.IsValid)
            {
                Context.Entry(newsarticle).State = EntityState.Modified;
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newsarticle);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticle newsarticle = Context.NewsArticles.Find(id);
            if (newsarticle == null)
            {
                return HttpNotFound();
            }
            return View(newsarticle);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsArticle newsarticle = Context.NewsArticles.Find(id);
            Context.NewsArticles.Remove(newsarticle);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
