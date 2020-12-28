using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class PasswordReset
    {
        [Required]
        [Display(Name = "Neues Passwort")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Passwort wiederholen")]
        public string PasswordConfirmation { get; set; }
    }
}