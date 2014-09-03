using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Document
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Aktualisiert")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime Timestamp { get; set; }
    }
}