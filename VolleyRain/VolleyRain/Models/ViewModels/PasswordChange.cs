using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class PasswordChange
    {
        [Required]
        [Display(Name = "Altes Passwort")]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "Neues Passwort")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Passwort wiederholen")]
        public string PasswordConfirmation { get; set; }
    }
}