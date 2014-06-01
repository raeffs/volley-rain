using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace VolleyRain.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public CustomIdentity CustomIdentity { get { return (CustomIdentity)Identity; } set { Identity = value; } }

        public bool IsInRole(string role)
        {
            // TODO: store roles in identity and check
            return Roles.IsUserInRole(Identity.Name, role);
        }

        public CustomPrincipal(CustomIdentity identity)
        {
            Identity = identity;
        }
    }
}