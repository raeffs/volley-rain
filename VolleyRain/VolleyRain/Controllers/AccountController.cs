using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
    }
}