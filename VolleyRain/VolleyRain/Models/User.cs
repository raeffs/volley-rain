using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public User()
        {
            Roles = new HashSet<Role>();
        }
    }
}