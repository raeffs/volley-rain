using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using VolleyRain.Models;
using VolleyRain.DataAccess;

namespace VolleyRain.Controllers
{
    public class NewsController : Controller
    {
        private const int DefaultPageSize = 2;

        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(db.NewsArticles.OrderByDescending(i => i.PublishDate).ToPagedList(currentPageIndex, DefaultPageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticle newsarticle = db.NewsArticles.Find(id);
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
                db.NewsArticles.Add(newsarticle);
                db.SaveChanges();
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
            NewsArticle newsarticle = db.NewsArticles.Find(id);
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
                db.Entry(newsarticle).State = EntityState.Modified;
                db.SaveChanges();
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
            NewsArticle newsarticle = db.NewsArticles.Find(id);
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
            NewsArticle newsarticle = db.NewsArticles.Find(id);
            db.NewsArticles.Remove(newsarticle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
