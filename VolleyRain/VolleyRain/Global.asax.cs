using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using VolleyRain.DataAccess;

namespace VolleyRain
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs args)
        {
            if (!FormsAuthentication.CookiesSupported || Request.Cookies[FormsAuthentication.FormsCookieName] == null) return;

            try
            {
                var username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                var roles = new string[] { };

                using (var db = new DatabaseContext())
                {
                    var user = db.Users.SingleOrDefault(u => u.Username == username);
                    roles = user.Roles.Select(r => r.Name).ToArray();
                }

                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(username, "Forms"), roles);
            }
            catch
            {

            }
        }
    }
}
