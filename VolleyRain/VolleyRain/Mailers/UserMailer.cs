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

        public virtual MvcMailMessage Welcome(User user)
        {
            ViewBag.User = user;
            return Populate(x =>
            {
                x.Subject = string.Format("Willkommen {0}", user.Name);
                x.ViewName = "Welcome";
                x.To.Add(user.Email);
            });
        }

        public virtual MvcMailMessage PasswordReset()
        {
            //ViewBag.Data = someObject;
            return Populate(x =>
            {
                x.Subject = "PasswordReset";
                x.ViewName = "PasswordReset";
                x.To.Add("some-email@example.com");
            });
        }
    }
}