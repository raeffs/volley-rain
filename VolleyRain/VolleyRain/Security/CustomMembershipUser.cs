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
            : base("CustomMembershipProvider", user.Email, user.ID, user.Email, string.Empty, string.Empty, user.IsApproved, user.IsLockedOut, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
        }
    }
}