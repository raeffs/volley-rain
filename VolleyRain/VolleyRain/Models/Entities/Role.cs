using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Role
    {
        public int ID { get; set; }

        public bool IsBuiltIn { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDefaultUserRole { get; set; }

        public bool IsDefaultAdminRole { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}