using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class DocumentController : BaseController
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            return View(Context.Documents.ToList());
        }

        [HttpGet]
        [Authorize]
        public ActionResult View(int documentID)
        {
            if (Context.Documents.None(d => d.ID == documentID)) return RedirectToAction("Index");

            var document = Context.Documents.Single(d => d.ID == documentID);
            return File(GetDocumentPath(document.FileName), MimeMapping.GetMimeMapping(document.FileName), document.Name);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(DocumentUpload model)
        {
            if (ModelState.IsValid)
            {
                var fileName = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                fileName = Path.ChangeExtension(fileName, Path.GetExtension(model.Document.FileName));
                model.Document.SaveAs(GetDocumentPath(fileName));

                Context.Documents.Add(new Document
                {
                    Name = string.IsNullOrWhiteSpace(model.Name)
                        ? model.Document.FileName
                        : Path.ChangeExtension(model.Name, Path.GetExtension(fileName)),
                    FileName = fileName,
                    Description = model.Description ?? string.Empty,
                    Timestamp = DateTime.Now,
                });
                Context.SaveChanges();

                TempData["SuccessMessage"] = "Dokument wurde hochgeladen.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Team-Administrator")]
        public ActionResult Delete(int documentID)
        {
            if (Context.Documents.None(d => d.ID == documentID)) return HttpNotFound();

            var model = Context.Documents.Single(d => d.ID == documentID);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Team-Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int documentID)
        {
            if (Context.Documents.None(d => d.ID == documentID)) return HttpNotFound();

            var model = Context.Documents.Single(d => d.ID == documentID);
            System.IO.File.Delete(GetDocumentPath(model.FileName));
            Context.Documents.Remove(model);
            Context.SaveChanges();
            TempData["SuccessMessage"] = "Das Dokument wurde gelöscht.";
            return RedirectToAction("Index");
        }


        private string GetDocumentPath(string fileName)
        {
            return Path.Combine(Server.MapPath("~/App_Data/Documents"), fileName);
        }
    }
}