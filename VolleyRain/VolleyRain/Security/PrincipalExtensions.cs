using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace VolleyRain.Extensions
{
    public static class PrincipalExtensions
    {
        public static bool IsAdministrator(this IPrincipal principal)
        {
            return principal.IsInRole("Administrator");
        }
    }
}