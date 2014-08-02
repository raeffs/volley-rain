using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VolleyRain.DataAccess;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class AccountController : BaseController
    {
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
            if (model.Password != model.PasswordConfirmation)
            {
                ModelState.AddModelError("PasswordConfirmation", "Die Passwörter stimmen nicht überein.");
            }
            if (Context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Die E-Mail-Adresse existiert bereits.");
            }

            if (ModelState.IsValid)
            {
                var entity = new User
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Password = model.Password,
                };
                entity.Roles.Add(Context.Roles.Single(r => r.IsDefaultUserRole));
                Context.Users.Add(entity);
                Context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }
    }
}