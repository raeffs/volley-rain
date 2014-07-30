using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        public bool IsLockedOut { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public User()
        {
            Roles = new List<Role>();
            Teams = new List<Team>();
        }
    }
}