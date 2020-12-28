using Mvc.Mailer;
using VolleyRain.Models;

namespace VolleyRain.Mailers
{
    public class UserMailer : MailerBase
    {
        public UserMailer()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Welcome(User user, string password)
        {
            ViewBag.User = user;
            ViewBag.Password = password;
            return Populate(x =>
            {
                x.Subject = string.Format("Willkommen {0}", user.Name);
                x.ViewName = "Welcome";
                x.To.Add(user.Email);
            });
        }

        public virtual MvcMailMessage PasswordReset(PasswordResetToken token)
        {
            ViewBag.Token = token;
            return Populate(x =>
            {
                x.Subject = "Anweisungen zum Zurücksetzen deines Passworts";
                x.ViewName = "PasswordReset";
                x.To.Add(token.User.Email);
            });
        }
    }
}