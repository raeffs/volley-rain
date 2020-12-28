using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class UserCreation
    {
        [Required]
        [Display(Name = "Vorname")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Nachname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "E-Mail wiederholen")]
        public string EmailConfirmation { get; set; }
    }
}