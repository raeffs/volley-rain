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
    public class NewsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: /News/
        public ActionResult Index()
        {
            return View(db.NewsArticles.ToList());
        }

        // GET: /News/Details/5
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

        // GET: /News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Title,Content,PublishDate")] NewsArticle newsarticle)
        {
            if (ModelState.IsValid)
            {
                db.NewsArticles.Add(newsarticle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newsarticle);
        }

        // GET: /News/Edit/5
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

        // POST: /News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Title,Content,PublishDate")] NewsArticle newsarticle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsarticle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newsarticle);
        }

        // GET: /News/Delete/5
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

        // POST: /News/Delete/5
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
