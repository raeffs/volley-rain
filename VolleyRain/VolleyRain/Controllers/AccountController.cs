using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using VolleyRain.DataAccess;
using VolleyRain.Mailers;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class AccountController : BaseController
    {
        private UserMailer mailer = new UserMailer();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return Logout();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Email, model.Password))
                {
                    FormsAuthentication.RedirectFromLoginPage(model.Email, model.RememberMe);
                }

                ModelState.AddModelError("", "Falsche E-Mail-Adresse und / oder Passwort.");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreation model)
        {
            if (Context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Die E-Mail-Adresse existiert bereits.");
            }
            if (model.Email != model.EmailConfirmation)
            {
                ModelState.AddModelError("EmailConfirmation", "Die E-Mail-Adressen stimmen nicht überein.");
            }

            if (ModelState.IsValid)
            {
                var generatedPassword = Membership.GeneratePassword(10, 2);
                var user = Membership.CreateUser(model.Email, generatedPassword, model.Email);

                var entity = Context.Users.Single(u => u.Email == user.Email);
                entity.Name = model.Name;
                entity.Surname = model.Surname;
                Context.SaveChanges();

                mailer.Welcome(entity, generatedPassword).SendAsync();

                TempData["SuccessMessage"] = "Dein Account wurde erstellt. Das Passwort wurde an die angegebene E-Mail-Adresse gesendet.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LostPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(PasswordResetRequest model)
        {
            if (Context.Users.None(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Die E-Mail-Adresse konnte nicht gefunden werden.");
            }
            if (ModelState.IsValid)
            {
                var user = Context.Users.Single(u => u.Email == model.Email);
                var generatedToken = Guid.NewGuid().ToString().Replace("-", "").ToUpper();

                var entity = new PasswordResetToken
                {
                    User = user,
                    Token = generatedToken,
                    ValidUntil = DateTime.Now.AddDays(1)
                };
                Context.PasswordResetTokens.Add(entity);
                Context.SaveChanges();

                mailer.PasswordReset(entity).SendAsync();

                TempData["SuccessMessage"] = "Eine E-Mail mit Anweisungen zum Zurücksetzen deines Passworts wurde an die angegebene E-Mail-Adresse gesendet.";
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || Context.PasswordResetTokens.None(t => t.Token == token && t.ValidUntil >= DateTime.Now))
            {
                TempData["ErrorMessage"] = "Der Link zum Zurücksetzen des Passworts ist nicht mehr gültig.";
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string token, PasswordReset model)
        {
            if (string.IsNullOrWhiteSpace(token) || Context.PasswordResetTokens.None(t => t.Token == token && t.ValidUntil >= DateTime.Now))
            {
                TempData["ErrorMessage"] = "Der Link zum Zurücksetzen des Passworts ist nicht mehr gültig.";
                return RedirectToAction("Login");
            }
            if (model.Password != model.PasswordConfirmation)
            {
                ModelState.AddModelError("PasswordConfirmation", "Die Passwörter stimmen nicht überein.");
            }

            if (ModelState.IsValid)
            {
                var entity = Context.PasswordResetTokens.Include(t => t.User).Single(t => t.Token == token);
                var user = Membership.GetUser(entity.User.Email);
                user.ChangePassword("dummy", model.Password);

                TempData["SuccessMessage"] = "Dein Passwort wurde geändert.";
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(PasswordChange model)
        {
            if (model.Password != model.PasswordConfirmation)
            {
                ModelState.AddModelError("PasswordConfirmation", "Die Passwörter stimmen nicht überein.");
            }
            if (!Membership.ValidateUser(Context.Users.Single(u => u.ID == Session.UserID).Email, model.CurrentPassword))
            {
                ModelState.AddModelError("CurrentPassword", "Das Passwort ist nicht korrekt.");
            }

            if (ModelState.IsValid)
            {
                var user = Membership.GetUser(Context.Users.Single(u => u.ID == Session.UserID).Email);
                user.ChangePassword("dummy", model.Password);

                TempData["SuccessMessage"] = "Dein Passwort wurde geändert.";
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var model = Context.Users
                .OrderBy(u => u.ID)
                .Select(u => new AccountSummary
                {
                    ID = u.ID,
                    Name = u.Name,
                    Surname = u.Surname,
                    Email = u.Email,
                    IsSelf = u.ID == Session.UserID
                })
                .ToList();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int accountID)
        {
            if (accountID == Session.UserID || Context.Users.None(u => u.ID == accountID)) return HttpNotFound();

            var model = Context.Users.Single(u => u.ID == accountID);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int accountID)
        {
            if (accountID == Session.UserID || Context.Users.None(u => u.ID == accountID)) return HttpNotFound();

            var model = Context.Users.Single(u => u.ID == accountID);
            Context.Users.Remove(model);
            Context.SaveChanges();
            TempData["successMessage"] = "Der Benutzer wurde gelöscht.";
            return RedirectToAction("Index");
        }
    }
}