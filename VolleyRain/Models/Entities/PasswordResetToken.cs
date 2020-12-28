using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class PasswordResetToken
    {
        public int ID { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ValidUntil { get; set; }
    }
}