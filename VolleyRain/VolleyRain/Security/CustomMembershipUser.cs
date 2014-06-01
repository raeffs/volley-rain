using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using VolleyRain.Models;

namespace VolleyRain.Security
{
    public class CustomMembershipUser : MembershipUser
    {
        public CustomMembershipUser(User user)
            : base("CustomMembershipProvider", user.Username, user.ID, string.Empty, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
        }
    }
}